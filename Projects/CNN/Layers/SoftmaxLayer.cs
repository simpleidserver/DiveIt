using CNN.Extensions;

namespace CNN.Layers
{
    public class SoftmaxLayer : ILayer
    {
        private readonly int _nbClasses;

        public SoftmaxLayer(int nbClasses)
        {
            _nbClasses = nbClasses;
        }

        public List<decimal[]>? Weights { get; set; } = null;
        public List<decimal>? Bias { get; set; } = null;

        public decimal[][,] Forward(decimal[][,] matrix)
        {
            InitWeights(matrix);
            InitBias();
            var flatten = MatrixHelper.Flat(matrix);
            var result = MatrixHelper.Sum(
                MatrixHelper.Multiply(flatten, Weights),
                Bias.ToArray());
            var totals = MatrixHelper.Exp(result);
            var sumExp = totals.Sum();
            return MatrixHelper.Divide(result, sumExp).Select(s =>
            {
                var rec = new decimal[1, 1];
                rec[0, 0] = s;
                return rec;
            }).ToArray();
        }

        private void InitBias()
        {
            if (Bias != null) return;
            Bias = new List<decimal>();
            var rnd = new Random();
            for(var m = 0; m < _nbClasses; m++)
            {
                Bias.Add(rnd.NextDecimal(-1, 1));
            }
        }

        private void InitWeights(decimal[][,] matrix)
        {
            if (Weights != null) return;
            Weights = new List<decimal[]>();
            var rnd = new Random();
            for(var m = 0; m < matrix.Length; m++)
            {
                var lst = new decimal[_nbClasses];
                for (var i = 0; i < _nbClasses; i++)
                    lst[i] = rnd.NextDecimal(-1, 1);
                Weights.Add(lst.ToArray());
            }
        }
    }
}
