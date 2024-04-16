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

        private double[][,] _lastInput;

        public double[][,] Forward(double[][,] matrixLst)
        {
            var result = new double[matrixLst.Length][,];
            for(var i = 0; i < matrixLst.Length; i++)
                result[i] = PoolingAlg.MaxPool(matrixLst[i], _poolWidth, _poolHeight);

            _lastInput = matrixLst;
            return result;
        }

        public void Backward()
        {

        }
    }
}