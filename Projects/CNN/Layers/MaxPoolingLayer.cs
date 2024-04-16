using CNN.Algs;

namespace CNN.Layers
{
    public class MaxPoolingLayer : ILayer
    {
        private readonly int _poolWidth;
        private readonly int _poolHeight;
        public MaxPoolingLayer(int poolWidth,
            int poolHeight)
        {
            _poolWidth = poolWidth;
            _poolHeight = poolHeight;
        }

        public decimal[][,] Forward(decimal[][,] matrixLst)
        {
            var result = new decimal[matrixLst.Length][,];
            for(var i = 0; i < matrixLst.Length; i++)
                result[i] = PoolingAlg.MaxPool(matrixLst[i], _poolWidth, _poolHeight);

            return result;
        }
    }
}