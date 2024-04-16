using CNN.Extensions;

namespace CNN.Layers
{
    public class SoftmaxLayer : ILayer
    {
        private readonly int _nbClasses;
        private double[][,] _lastInput;
        private double[] _lastTotals;

        public SoftmaxLayer(int nbClasses)
        {
            _nbClasses = nbClasses;
        }

        public List<double[]>? Weights { get; set; } = null;
        public List<double>? Bias { get; set; } = null;

        public double[][,] Forward(double[][,] matrix)
        {
            _lastInput = matrix;
            InitWeights(matrix);
            InitBias();
            var flatten = MatrixHelper.Flat(matrix);
            var result = MatrixHelper.Sum(
                MatrixHelper.Multiply(flatten, Weights),
                Bias.ToArray());
            var totals = MatrixHelper.Exp(result);
            _lastTotals = totals;
            var sumExp = totals.Sum();
            return MatrixHelper.Divide(result, sumExp).Select(s =>
            {
                var rec = new double[1, 1];
                rec[0, 0] = s;
                return rec;
            }).ToArray();
        }

        private void InitBias()
        {
            if (Bias != null) return;
            Bias = new List<double>();
            var rnd = new Random();
            for(var m = 0; m < _nbClasses; m++)
            {
                Bias.Add(rnd.NextDouble(-1, 1));
            }
        }

        private void InitWeights(double[][,] matrix)
        {
            if (Weights != null) return;
            Weights = new List<double[]>();
            var rnd = new Random();
            for(var m = 0; m < matrix.Length; m++)
            {
                var lst = new double[_nbClasses];
                for (var i = 0; i < _nbClasses; i++)
                    lst[i] = rnd.NextDouble(-1, 1);
                Weights.Add(lst.ToArray());
            }
        }
    }
}
