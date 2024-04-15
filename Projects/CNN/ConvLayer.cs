namespace CNN
{
    public class ConvLayer : ILayer
    {
        private List<ConvLayerNeuron> _neurons = new List<ConvLayerNeuron>();

        public ConvLayer(
            int nbFilters, 
            int kernelSize,
            int padding = 1,
            int strideW = 1,
            int strideH = 1)
        {
            for(var o = 0; o < nbFilters; o++)
                _neurons.Add(new ConvLayerNeuron(kernelSize, padding, strideW, strideH));
        }

        public decimal[][,] Forward(decimal[][,] matrixLst)
        {
            var result = new decimal[_neurons.Count][,];
            for (var i = 0; i < _neurons.Count(); i++)
                result[i] = _neurons[i].Forward(matrixLst);
            return result;
        }

        public void Backward(int[][,] matrixes)
        {

        }

        private class ConvLayerNeuron
        {
            private readonly int _kernelSize;
            private readonly int _padding;
            private readonly int _strideW;
            private readonly int _strideH;

            public ConvLayerNeuron(int kernelSize, int padding = 1, int strideW = 1, int strideH = 1)
            {
                _kernelSize = kernelSize;
                _padding = padding;
                _strideW = strideW;
                _strideH = strideH;
            }

            public List<decimal[,]> Weights { get; private set; }

            public decimal[,] Bias { get; set; }

            public decimal[,] Forward(decimal[][,] matrixLst)
            {
                // SUM(Weight & matrix) + bias
                InitWeights(matrixLst.Count());
                InitBias(matrixLst.First());
                var result = (decimal[,])Bias.Clone();
                for (var i = 0; i < matrixLst.Count(); i++)
                {
                    var matrix = matrixLst[i];
                    var convolutionResult = ConvolutionAlg.Transform(
                        matrix, 
                        Weights[i],
                        _padding,
                        _strideW,
                        _strideH);
                    result = MatrixHelper.Sum(result, convolutionResult);
                }

                return result;
            }

            private void InitWeights(int inputLength)
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

            private void InitBias(decimal[,] matrix)
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
                for(var y =  0; y < outputShape.height; y++)
                {
                    for(var x = 0; x < outputShape.width; x++)
                    {
                        Bias[y, x] = rnd.NextDecimal(-1, 1);
                    }
                }
            }
        }
    }
}
