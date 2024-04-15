namespace CNN;

public class Pooling
{
    public static int[,] MaxPool(int[,] inputMatrix, int poolWidth, int poolHeight)
    {
        var result = new int[
            (int)Math.Round((decimal)(inputMatrix.GetLength(0) / poolHeight)),
            (int)Math.Round((decimal)(inputMatrix.GetLength(1) / poolWidth))
        ];
        for(var y = 0; y < result.GetLength(0); y++)
        {
            for(var x = 0; x < result.GetLength(1); x++)
            {
                var portion = MatrixHelper.GetPortion(inputMatrix, poolWidth, poolHeight, x * poolWidth, y * poolHeight);
                result[y,x] = MatrixHelper.Max(portion);
            }
        }

        return result;
    }
}
