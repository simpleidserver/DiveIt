namespace CNN.Layers
{
    public abstract class BaseLayerNeuron : ILayerNeuron
    {
        public List<decimal[,]>? Weights { get; protected set; } = null;

        public decimal[,]? Bias { get; set; } = null;

        public decimal[,] Forward(decimal[][,] matrixLst)
        {
            InitWeights(matrixLst.Count());
            InitBias(matrixLst.First());
            var result = (decimal[,])Bias.Clone();
            for (var i = 0; i < matrixLst.Count(); i++)
            {
                result = MatrixHelper.Sum(result, Forward(matrixLst[i], i));
            }

            return result;
        }

        protected abstract decimal[,] Forward(decimal[,] matrix, int index);

        protected abstract void InitWeights(int inputLength);

        protected abstract void InitBias(decimal[,] matrix);
    }
}
