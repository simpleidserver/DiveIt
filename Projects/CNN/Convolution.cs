using System.Threading.Tasks.Dataflow;

namespace CNN;

public class ConvolutionPicture
{
    public static int[,] Transform(int[,] picture, int[,] filter)
    {
        int stride = 1; // TODO : use this hyperparameter.
        int bias = 1; // TODO : use this hyperparameter.
        var height = picture.GetLength(0);
        var width = picture.GetLength(1);
        var filterSize = filter.GetLength(0);
        var nbHeightIterations = (int)Math.Round((decimal)height / filterSize);
        var nbWithIterations = (int)Math.Round((decimal)(width / filterSize));
        var result = new int[nbHeightIterations, nbWithIterations];
        for(var y = 0; y < nbHeightIterations; y++)
        {
            for(var x = 0; x < nbWithIterations; x++)
            {
                var portion = GetPicturePortion(picture, x, y, filterSize);
                result[y, x] = Multiply(portion, filter);
            }
        }

        return result;
    }

    private static int[,] GetPicturePortion(int[,] picture, int startX, int startY, int filterSize)
    {
        var result = new int[filterSize, filterSize];
        for(var y = 0; y < filterSize; y++)
            for(var x = 0; x < filterSize; x++)
                result[y,x] = picture[(startY * filterSize) + y, (startX * filterSize) + x];

        return result;
    }

    private static int Multiply(int[,] matrixA, int[,] matrixB)
    {
        var height = matrixA.GetLength(0);
        var width = matrixA.GetLength(1);
        int result = 0;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                result = matrixA[y,x] * matrixB[y,x];
            }
        }

        return result;
    }
}
