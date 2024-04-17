using CNN.Extensions;

namespace CNN.Layers
{
    public class SoftmaxLayer : ILayer
    {
        private readonly int _nbClasses;
        private int _lastInputNbInput = 0;
        private int _lastInputHeight = 0;
        private int _lastInputWidth = 0;
        private double[]? _lastInput = null;
        private double[] _lastTotals;

        public SoftmaxLayer(int nbClasses)
        {
            _nbClasses = nbClasses;
        }

        public double LearningRate { get; set; } = 0.005;
        public double[,]? Weights { get; set; } = null;
        public double[]? Bias { get; set; } = null;

        public double[,,] Forward(double[,,] matrix)
        {
            InitWeights(matrix);
            InitBias();
            _lastInputNbInput = matrix.GetLength(0);
            _lastInputHeight = matrix.GetLength(1);
            _lastInputWidth = matrix.GetLength(2);
            var flatten = ArrayHelper.Flat(matrix);
            _lastInput = flatten;
            var result = ArrayHelper.Sum(
                ArrayHelper.Multiply(flatten, Weights),
                Bias);
            _lastTotals = result;
            var totals = ArrayHelper.Exp(result);
            var sumExp = totals.Sum();
            var r = ArrayHelper.Divide(result, sumExp);
            return ArrayHelper.TransformTo3D(r);
        }

        public double[,,] Backward(double[,,] matrix)
        {
            var gradients = new double[matrix.GetLength(2)];
            for(var n  = 0; n < matrix.GetLength(2); n++)
                gradients[n] = matrix[0, 0, n];
            for(var i = 0; i < gradients.Count(); i++)
            {
                var gradient = gradients.ElementAt(i);
                if (gradient == 0) continue;

                var expTotals = ArrayHelper.Exp(_lastTotals);
                var s = expTotals.Sum();
                // gradient of output against totals.
                var gradientOutput = ArrayHelper.Divide(
                    ArrayHelper.Multiply(expTotals, -expTotals[i]),
                    s * s); // k != c
                gradientOutput[i] = expTotals[i] * (s - expTotals[i]) / (s * s); // k == c

                // gradient of totals against weights / bias / input.
                var gradientWeight = _lastInput;
                var gradientBias = 1;
                var gradientInputs = Weights;

                // gradient of loss against totals.
                var gradientLossTotal = ArrayHelper.Multiply(gradientOutput, gradient);

                // gradient of loss against weights / bias / input. 
                var gradientLossWeights = ArrayHelper.TransposeAndMultiplyWithNewAxis(gradientWeight, gradientLossTotal);
                var gradientLossBias = ArrayHelper.Multiply(gradientLossTotal, gradientBias);
                var gradientLossInputs = ArrayHelper.MultiplyAndSum(gradientInputs, gradientLossTotal);

                Weights = ArrayHelper.Substract(
                    Weights,
                    ArrayHelper.Multiply(gradientLossWeights, LearningRate));
                Bias = ArrayHelper.Substract(
                    Bias,
                    ArrayHelper.Multiply(gradientLossBias, LearningRate));

                return ArrayHelper.TransformTo3D(
                    gradientLossInputs, 
                    _lastInputNbInput, 
                    _lastInputWidth, 
                    _lastInputHeight);
            }

            return new double[0,0,0];
        }

        private void InitBias()
        {
            if (Bias != null) return;
            Bias = new double[_nbClasses];
            var rnd = new Random();
            for(var m = 0; m < _nbClasses; m++)
                Bias[m] = rnd.NextDouble(-1, 1);
        }

        private void InitWeights(double[,,] matrix)
        {
            if (Weights != null) return;
            var ln = matrix.GetLength(0) * matrix.GetLength(1) * matrix.GetLength(2);
            Weights = new double[ln,_nbClasses];
            var rnd = new Random();
            for(var m = 0; m < ln; m++)
            {
                for (var i = 0; i < _nbClasses; i++)
                    Weights[m, i] = rnd.NextDouble(-1, 1) / ln;
            }
        }
    }
}
