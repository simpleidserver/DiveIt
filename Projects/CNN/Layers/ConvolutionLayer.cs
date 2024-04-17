using CNN.Algs;
using CNN.Extensions;

namespace CNN.Layers
{
    public class ConvolutionLayer : ILayer
    {
        private readonly int _nbFilters;
        private readonly int _kernelSize;
        private readonly int _padding;
        private readonly int _strideW;
        private readonly int _strideH;

        public ConvolutionLayer(
            int nbFilters,
            int kernelSize,
            int padding = 1,
            int strideW = 1,
            int strideH = 1)
        {
            _nbFilters = nbFilters;
            _kernelSize = kernelSize;
            _padding = padding;
            _strideW = strideW;
            _strideH = strideH;
            for (var o = 0; o < nbFilters; o++)
                Neurons.Add(new ConvolutionLayerNeuron(kernelSize));
        }

        public List<ConvolutionLayerNeuron> Neurons { get; set; } = new List<ConvolutionLayerNeuron>();

        public double[,,] Forward(double[,,] matrix)
        {
            // take only the first matrix.
            var firstMatrix = matrix.TransformFirstRecordInto2DArray();
            var outputShape = ConvolutionAlg.GetOutputShape(firstMatrix,
                (width: _kernelSize, height: _kernelSize),
                _padding,
                _strideW,
                _strideH);
            var portions = ConvolutionAlg.GetPortions(firstMatrix,
                (width: _kernelSize, height: _kernelSize),
                _padding,
                _strideW,
                _strideH);
            var result = new double[outputShape.height, outputShape.width, _nbFilters];
            foreach (var portion in portions)
            {
                // all filters * region
                // sum each region and obtain an array.
                for (var n = 0; n < Neurons.Count; n++)
                {
                    var neuron = Neurons[n];
                    var conv = neuron.Forward(portion.Item1);
                    result[portion.Item3, portion.Item2, n] = conv;
                }
            }

            // TODO : Check the implementation is correct !!!
            return result;
        }

        public double[,,] Backward(double[,,] matrix)
        {
            throw new NotImplementedException();
        }
    }

    public class ConvolutionLayerNeuron
    {
        private readonly int _kernelSize;

        public ConvolutionLayerNeuron(
            int kernelSize)
        {
            _kernelSize = kernelSize;
            InitWeights();
        }

        public double[,]? Weights { get; protected set; } = null;

        public double Forward(double[,] matrix)
        {
            return ArrayHelper.Conv(matrix, Weights);
        }

        private void InitWeights()
        {
            var rnd = new Random();
            Weights = new double[_kernelSize, _kernelSize];
            for (var x = 0; x < _kernelSize; x++)
            {
                for (var y = 0; y < _kernelSize; y++)
                {
                    rnd.Next(-1, 1);
                    Weights[y, x] = rnd.NextDouble(-1, 1) / 9;
                }
            }
        }
    }
}
