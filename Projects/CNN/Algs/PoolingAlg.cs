namespace CNN.Algs;

public class PoolingAlg
{
    public static double[,] MaxPool(double[,] inputMatrix, int poolWidth, int poolHeight)
    {
        var result = new double[
            (int)Math.Round((double)(inputMatrix.GetLength(0) / poolHeight)),
            (int)Math.Round((double)(inputMatrix.GetLength(1) / poolWidth))
        ];
        for (var y = 0; y < result.GetLength(0); y++)
        {
            for (var x = 0; x < result.GetLength(1); x++)
            {
                var portion = MatrixHelper.GetPortion(inputMatrix, poolWidth, poolHeight, x * poolWidth, y * poolHeight);
                result[y, x] = MatrixHelper.Max(portion);
            }
        }

        return result;
    }
}
