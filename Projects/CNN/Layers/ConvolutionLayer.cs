using CNN.Algs;
using CNN.Extensions;

namespace CNN.Layers
{
    public class ConvolutionLayer : BaseLayer<ConvolutionLayerNeuron>
    {
        public ConvolutionLayer(
            int nbFilters,
            int kernelSize,
            int padding = 1,
            int strideW = 1,
            int strideH = 1)
        {
            for (var o = 0; o < nbFilters; o++)
                Neurons.Add(new ConvolutionLayerNeuron(kernelSize, padding, strideW, strideH));
        }
    }

    public class ConvolutionLayerNeuron : BaseLayerNeuron
    {
        private readonly int _kernelSize;
        private readonly int _padding;
        private readonly int _strideW;
        private readonly int _strideH;

        public ConvolutionLayerNeuron(
            int kernelSize, 
            int padding = 1, 
            int strideW = 1, 
            int strideH = 1)
        {
            _kernelSize = kernelSize;
            _padding = padding;
            _strideW = strideW;
            _strideH = strideH;
        }

        protected override decimal[,] Forward(decimal[,] matrix, int index)
        {
            var convolutionResult = ConvolutionAlg.Transform(
                matrix,
                Weights[index],
                _padding,
                _strideW,
                _strideH);
            return convolutionResult;
        }

        protected override void InitWeights(int inputLength)
        {
            if (Weights != null) return;
            Weights = new List<decimal[,]>();
            var rnd = new Random();
            for (var i = 0; i < inputLength; i++)
            {
                var kernel = new decimal[_kernelSize, _kernelSize];
                for (var x = 0; x < _kernelSize; x++)
                {
                    for (var y = 0; y < _kernelSize; y++)
                    {
                        rnd.Next(-1, 1);
                        kernel[y, x] = rnd.NextDecimal(-1, 1);
                    }
                }

                Weights.Add(kernel);
            }
        }

        protected override void InitBias(decimal[,] matrix)
        {
            if (Bias != null) return;
            var outputShape = ConvolutionAlg.GetOutputShape(
                matrix,
                Weights.First(),
                _padding,
                _strideW,
                _strideH);
            Bias = new decimal[outputShape.height, outputShape.width];
            var rnd = new Random();
            for (var y = 0; y < outputShape.height; y++)
            {
                for (var x = 0; x < outputShape.width; x++)
                {
                    Bias[y, x] = rnd.NextDecimal(-1, 1);
                }
            }
        }
    }
}
