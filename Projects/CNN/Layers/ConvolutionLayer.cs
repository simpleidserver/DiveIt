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
        private double[,,] _lastInput;

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

        public double LearningRate { get; set; } = 0.005;

        public List<ConvolutionLayerNeuron> Neurons { get; set; } = new List<ConvolutionLayerNeuron>();

        public double[,,] Forward(double[,,] matrix)
        {
            // take only the first matrix.
            _lastInput = matrix;
            var firstMatrix = ArrayHelper.TransformFirstRecordInto2DArray(matrix);
            var outputShape = GetOutputShape(firstMatrix,
                (width: _kernelSize, height: _kernelSize),
                _padding,
                _strideW,
                _strideH);
            var portions = GetPortions(firstMatrix,
                (width: _kernelSize, height: _kernelSize),
                _padding,
                _strideW,
                _strideH);
            var result = new double[outputShape.height, outputShape.width, _nbFilters];
            foreach (var portion in portions)
            {
                for (var n = 0; n < Neurons.Count; n++)
                {
                    var neuron = Neurons[n];
                    var conv = neuron.Forward(portion.Item1);
                    result[portion.Item3, portion.Item2, n] = conv;
                }
            }

            return result;
        }

        public double[,,] Backward(double[,,] matrix)
        {
            var firstMatrix = ArrayHelper.TransformFirstRecordInto2DArray(_lastInput);
            var portions = GetPortions(firstMatrix,
                (width: _kernelSize, height: _kernelSize),
                _padding,
                _strideW,
                _strideH);
            var gradientWeightNeurons = new Dictionary<int, double[,]>();
            foreach(var portion in portions)
            {
                for(var f = 0; f < Neurons.Count; f++)
                {
                    var r = ArrayHelper.Multiply(
                        portion.Item1,
                        matrix[portion.Item3, portion.Item2, f]
                    );
                    
                    if(!gradientWeightNeurons.ContainsKey(f))
                        gradientWeightNeurons.Add(f, new double[_kernelSize, _kernelSize]);
                    gradientWeightNeurons[f] = ArrayHelper.Sum(gradientWeightNeurons[f], r);
                }
            }

            for(var f = 0; f < Neurons.Count; f++)
            {
                var neuron = Neurons[f];
                neuron.Weights = ArrayHelper.Substract(neuron.Weights,
                    ArrayHelper.Multiply(gradientWeightNeurons[f], LearningRate)
                );
            }

            return null;
        }

        private static List<(double[,], int, int)> GetPortions(
            double[,] picture,
            (int width, int height) filterShape,
            int padding = 1,
            int strideW = 1,
            int strideH = 1)
        {
            var resizedPicture = ResizePicture(picture, padding);
            var outputShape = GetOutputShape(picture, filterShape, padding, strideW, strideH);
            var result = new List<(double[,], int, int)>();
            for (var y = 0; y < outputShape.height; y++)
            {
                for (var x = 0; x < outputShape.width; x++)
                {
                    var portion = ArrayHelper.GetPortion(
                        resizedPicture,
                        filterShape.width,
                        filterShape.height,
                        x * strideW,
                        y * strideH);
                    result.Add((portion, x, y));
                }
            }

            return result;
        }

        private static (int height, int width) GetOutputShape(
            double[,] picture,
            (int width, int height) filterShape,
            int padding = 1,
            int strideW = 1,
            int strideH = 1)
        {
            return (height: (picture.GetLength(0) - filterShape.height + padding * 2 + strideH) / strideH,
                width: (picture.GetLength(1) - filterShape.width + padding * 2 + strideW) / strideW);
        }

        private static double[,] ResizePicture(double[,] picture, int padding)
        {
            var newHeight = picture.GetLength(0) + padding * 2;
            var newWidth = picture.GetLength(1) + padding * 2;
            var result = new double[newHeight, newWidth];
            for (var y = 0; y < picture.GetLength(0); y++)
                for (var x = 0; x < picture.GetLength(1); x++)
                    result[y + padding, x + padding] = picture[y, x];

            return result;
        }
    }

    public class ConvolutionLayerNeuron
    {
        private readonly int _kernelSize;

        public ConvolutionLayerNeuron()
        {
            
        }

        public ConvolutionLayerNeuron(
            int kernelSize)
        {
            _kernelSize = kernelSize;
            InitWeights();
        }

        public double[,]? Weights { get; set; } = null;

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
                    Weights[y, x] = rnd.NextDouble(-1, 1) / 9;
                }
            }
        }
    }
}
