namespace CNN.Layers
{
    public abstract class BaseLayerNeuron : ILayerNeuron
    {
        public List<double[,]>? Weights { get; protected set; } = null;

        public double[,]? Bias { get; set; } = null;

        public double[,] Forward(double[][,] matrixLst)
        {
            InitWeights(matrixLst.Count());
            InitBias(matrixLst.First());
            var result = (double[,])Bias.Clone();
            for (var i = 0; i < matrixLst.Count(); i++)
            {
                result = MatrixHelper.Sum(result, Forward(matrixLst[i], i));
            }

            return result;
        }

        protected abstract double[,] Forward(double[,] matrix, int index);

        protected abstract void InitWeights(int inputLength);

        protected abstract void InitBias(double[,] matrix);
    }
}
