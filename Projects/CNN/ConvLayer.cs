namespace CNN
{
    public class ConvLayer
    {
        private List<ConvLayerNeuron> _neurons = new List<ConvLayerNeuron>();

        public ConvLayer(
            int inputLength, 
            int outputLength, 
            int kernelSize)
        {
            for(var o = 0; o < outputLength; o++)
                _neurons.Add(new ConvLayerNeuron(kernelSize, inputLength));
        }

        public int[][,] Forward(int[][,] matrixLst)
        {
            var result = new int[_neurons.Count][,];
            for (var i = 0; i < _neurons.Count(); i++)
                result[i] = _neurons[i].Forward(matrixLst);
            return result;
        }

        public void Backward(int[][,] matrixes)
        {

        }

        private class ConvLayerNeuron
        {
            private readonly int _inputLength;

            public ConvLayerNeuron(int kernelSize, int inputLength)
            {
                _inputLength = inputLength;
                InitWeights(kernelSize, inputLength);
            }

            public Dictionary<int, int[,]> Weights { get; private set; } = new Dictionary<int, int[,]>();

            public int[,] Forward(int[][,] matrixLst)
            {
                // SUM(Weight & matrix) + bias
                var firstMatrix = matrixLst.First();
                var result = new int[
                    firstMatrix.GetLength(0), 
                    firstMatrix.GetLength(1)
                ];
                for (var i = 0; i < _inputLength; i++)
                {
                    var matrix = matrixLst[i];
                    var convolutionResult = ConvolutionAlg.Transform(matrix, Weights[i]);
                    result = MatrixHelper.Sum(result, convolutionResult);
                }

                return result;
            }

            private void InitWeights(int kernelSize, int inputLength)
            {
                Weights = new Dictionary<int, int[,]>();
                var rnd = new Random();
                for (var i = 0; i < inputLength; i++)
                {
                    var kernel = new int[kernelSize, kernelSize];
                    for (var x = 0; x < kernelSize; x++)
                    {
                        for (var y = 0; y < kernelSize; y++)
                        {
                            rnd.Next(-1, 1);
                            kernel[y, x] = rnd.Next(-3, 3);
                        }
                    }

                    Weights.Add(i, kernel);
                }
            }

            private void InitBias()
            {

            }
        }
    }
}
