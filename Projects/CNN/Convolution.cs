namespace CNN;

public class ConvolutionPicture
{
    public static int[,] Transform(int[,] picture, int[,] filter, int padding = 1)
    {
        var height = picture.GetLength(0);
        var width = picture.GetLength(1);
        picture = ResizePicture(picture, padding);
        var result = new int[
            picture.GetLength(0) - filter.GetLength(0) + padding - 1, 
            picture.GetLength(1) - filter.GetLength(1) + padding - 1
        ];
        for(var y = 0; y < result.GetLength(0); y++)
        {
            for(var x = 0; x < result.GetLength(1); x++)
            {
                var portion = GetPicturePortion(picture, filter, x, y);
                result[y, x] = Multiply(portion, filter);
            }
        }

        return result;
    }

    public static int[,] ResizePicture(int[,] picture, int padding)
    {
        var newHeight = picture.GetLength(0) + padding * 2;
        var newWidth = picture.GetLength(1) + padding * 2;
        var result = new int[newHeight, newWidth];
        for(var y = 0; y < picture.GetLength(0); y++)
            for(var x = 0; x < picture.GetLength(1); x++)
                result[y + padding, x + padding] = picture[y, x];

        return result;
    }

    private static int[,] GetPicturePortion(int[,] picture, int[,] filter, int startX, int startY)
    {
        var result = new int[filter.GetLength(0), filter.GetLength(1)];
        var filterSize = filter.GetLength(0);
        /*
        var mod = filterSize % 2;
        var itStartY = startY - filterSize;
        var itEndY = startY;
        var itStartX = startX - filterSize;
        var itEndX = startX;
        if (mod == 1)
        {
            var s = (filterSize - 1) / 2;
            itStartY = startY - s;
            itEndY = startY + s;
            itStartX = startX - s;
            itEndX = startX + s;
        }
        */

        for(var y = 0; y < filter.GetLength(0); y++)
            for(var x = 0; x < filter.GetLength(1); x++)
            {
                result[y, x] = picture[startY + y, startX + x];
            }

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
                result += matrixA[y,x] * matrixB[y,x];
            }
        }

        return result;
    }
}
