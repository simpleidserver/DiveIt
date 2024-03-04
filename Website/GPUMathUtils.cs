using ILGPU;
using ILGPU.Runtime;
using ILGPU.Runtime.Cuda;

namespace SGDOptimization;

public class GPUMathUtils
{
    private static CudaAccelerator _accelerator;

    public static void Init()
    {
        Context context = Context.CreateDefault();
        _accelerator = context.CreateCudaAccelerator(0);
    }

    public static double[] Multiply(double[,] matrix, double[] vector)
    {
        var nbRow = matrix.GetLength(0);
        var nbCol = matrix.GetLength(1);
        ArrayView<double> xBuffer = _accelerator.Allocate1D<double>(nbRow * nbCol)
            .AsArrayView<double>(0, nbRow * nbCol);
        var yBuffer = _accelerator.Allocate1D<double, Stride1D.General>(vector.Count(), new Stride1D.General());
        var x = new double[nbRow * nbCol];
        for (var i = 0; i < nbRow; i++)
            for (var y = 0; y < nbCol; y++)
                x[i + y] = matrix[i, y];

        var outputBuffer = _accelerator.Allocate1D<double, Stride1D.General>(matrix.GetLength(0), new Stride1D.General());

        xBuffer.CopyFromCPU(x);
        yBuffer.CopyFromCPU(vector);

        double alpha = 1.0;
        double beta = 0;
        var blas = new CuBlas(_accelerator);
        blas.Gemv(CuBlasOperation.NonTranspose,
            nbRow,
            nbCol,
            alpha,
            xBuffer,
            nbRow,
            yBuffer.View,
            beta,
            outputBuffer.View);
        _accelerator.Synchronize();

        var arr = outputBuffer.GetAsArray1D();
        return null;
    }
}
