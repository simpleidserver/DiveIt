using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;

namespace SGDOptimization;

public class GPUMathUtils : IMathUtils, IDisposable
{
    private static GPUMathUtils _instance;
    private CudaAccelerator _accelerator;
    private CuBlas _blas;
    private Action<Index1D, ArrayView1D<double, Stride1D.General>, ArrayView1D<double, Stride1D.General>> _substractVector;

    private GPUMathUtils()
    {
        Context context = Context.CreateDefault();
        _accelerator = context.CreateCudaAccelerator(0);
        _blas = new CuBlas(_accelerator);
        _substractVector = _accelerator.LoadAutoGroupedStreamKernel<Index1D, ArrayView1D<double, Stride1D.General>, ArrayView1D<double, Stride1D.General>>(VectorSubstractVectorKernel);
    }

    public static GPUMathUtils New()
    {
        if (_instance == null)
            _instance = new GPUMathUtils();
        return _instance;
    }

    public SgdResult Sgd(double[,] x, double[] y, double[] w, double itp)
    {
        var nbRow = x.GetLength(0);
        var nbCol = x.GetLength(1);
        using (var xBuffer = _accelerator.Allocate1D<double>(nbRow * nbCol))
        {
            var xBufferView = xBuffer.AsArrayView<double>(0, nbRow * nbCol);
            using (var wBuffer = _accelerator.Allocate1D<double>(w.Count()))
            {
                using (var yBuffer = _accelerator.Allocate1D<double>(y.Count()))
                {
                    using (var outputBuffer = _accelerator.Allocate1D<double>(nbRow))
                    {
                        using(var weightGradientBuffer = _accelerator.Allocate1D<double>(nbCol))
                        {
                            var flatX = ColumnMajorFormat(x);
                            var flatItp = Enumerable.Repeat(itp, nbRow).ToArray();
                            xBuffer.CopyFromCPU(flatX);
                            wBuffer.CopyFromCPU(w);
                            yBuffer.CopyFromCPU(y);
                            outputBuffer.CopyFromCPU(flatItp);
                            var outputBufferView = outputBuffer.View.AsGeneral();
                            var yBufferView = yBuffer.View.AsGeneral();
                            var weightGradientBufferView = weightGradientBuffer.View.AsGeneral();

                            double alpha = 1.0;
                            double beta = 1.0;

                            // newy = itp + w * x
                            _blas.Gemv(CuBlasOperation.NonTranspose,
                                nbRow,
                                nbCol,
                                alpha,
                                xBufferView,
                                nbRow,
                                wBuffer.View.AsGeneral(),
                                beta,
                                outputBufferView);

                            // lostfactor = y - newy
                            _substractVector(yBufferView.IntLength,
                                yBufferView,
                                outputBufferView);

                            beta = 0.0;
                            alpha = -2.0;
                            // weight = -2 * (xi * losefactor)
                            _blas.Gemv(CuBlasOperation.Transpose,
                                nbRow,
                                nbCol,
                                alpha,
                                xBufferView,
                                nbRow,
                                outputBufferView,
                                beta,
                                weightGradientBufferView);

                            // -2 * losefactor
                            _blas.Scal(-2, outputBufferView);

                            _accelerator.Synchronize();
                            var weight = weightGradientBufferView.GetAsArray1D();
                            var itpArray = outputBufferView.GetAsArray1D();
                            return new SgdResult(weight, itpArray.Sum());
                        }
                    }
                }
            }
        }
    }

    private static void VectorSubstractVectorKernel(
        Index1D index,
        ArrayView1D<double, Stride1D.General> aView,
        ArrayView1D<double, Stride1D.General> bView)
    {
        bView[index] = aView[index] - bView[index];
    }

    private static double[] ColumnMajorFormat(double[,] matrix)
    {
        var nbRow = matrix.GetLength(0);
        var nbCol = matrix.GetLength(1);
        var result = new double[nbRow * nbCol];
        int index = 0;
        for (var i = 0; i < nbCol; i++)
            for (var y = 0; y < nbRow; y++)
            {
                result[index] = matrix[y, i];
                index++;
            }

        return result;
    }

    public void Dispose()
    {
        _blas.Dispose();
        _accelerator.Dispose();
    }
}
