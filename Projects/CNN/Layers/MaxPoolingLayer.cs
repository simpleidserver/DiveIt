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

        public double[,,] Forward(double[,,] matrixLst)
        {
            var newHeight = (int)Math.Round((double)(matrixLst.GetLength(0) / _poolHeight));
            var newWidth = (int)Math.Round((double)(matrixLst.GetLength(1) / _poolWidth));
            var result = new double[newHeight, newWidth, matrixLst.GetLength(2)];
            for (var y = 0; y < newHeight; y++)
            {
                for(var x = 0; x < newWidth; x++)
                {
                    var portion = ArrayHelper.GetPortion(matrixLst,
                        _poolWidth,
                        _poolHeight,
                        x * _poolWidth,
                        y * _poolHeight);
                    var max = ArrayHelper.Max(portion);
                    for(var n = 0; n < max.Length; n++)
                        result[y, x, n] = max[n];
                }
            }

            return result;
        }

        public double[,,] Backward(double[,,] matrix)
        {
            return null;
        }
    }
}