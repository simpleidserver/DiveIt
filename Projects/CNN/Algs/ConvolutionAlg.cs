namespace CNN.Algs;

public class ConvolutionAlg
{
    public static List<(double[,],int,int)> GetPortions(
        double[,] picture,
        (int width, int height) filterShape, 
        int padding = 1, 
        int strideW = 1, 
        int strideH = 1)
    {
        var resizedPicture = ResizePicture(picture, padding);
        var outputShape = GetOutputShape(picture, filterShape, padding, strideW, strideH);
        var result = new List<(double[,],int,int)>();
        for (var y = 0; y < outputShape.height; y++)
        {
            for (var x = 0; x < outputShape.width; x++)
            {
                var portion = ArrayHelper.GetPortion(
                    resizedPicture,
                    filterShape.width,
                    filterShape.height,
                    x * strideW,
                    y * strideH);
                result.Add((portion, x, y));
            }
        }

        return result;
    }

    /*
    public static double[,] Transform(double[,] picture, double[,] filter, int padding = 1, int strideW = 1, int strideH = 1)
    {
        var resizedPicture = ResizePicture(picture, padding);
        var outputShape = GetOutputShape(picture, filter, padding, strideW, strideH);
        var result = new double[
            outputShape.height,
            outputShape.width
        ];
        for (var y = 0; y < result.GetLength(0); y++)
        {
            for (var x = 0; x < result.GetLength(1); x++)
            {
                var portion = ArrayHelper.GetPortion(
                    resizedPicture,
                    filter.GetLength(1),
                    filter.GetLength(0),
                    x * strideW,
                    y * strideH);
                result[y, x] = ArrayHelper.Multiply(portion, filter);
            }
        }

        return result;
    }
    */

    public static (int height, int width) GetOutputShape(
        double[,] picture, 
        (int width, int height) filterShape, 
        int padding = 1, 
        int strideW = 1, 
        int strideH = 1)
    {
        return (height: (picture.GetLength(0) - filterShape.height + padding * 2 + strideH) / strideH,
            width: (picture.GetLength(1) - filterShape.width + padding * 2 + strideW) / strideW);
    }

    public static double[,] ResizePicture(double[,] picture, int padding)
    {
        var newHeight = picture.GetLength(0) + padding * 2;
        var newWidth = picture.GetLength(1) + padding * 2;
        var result = new double[newHeight, newWidth];
        for (var y = 0; y < picture.GetLength(0); y++)
            for (var x = 0; x < picture.GetLength(1); x++)
                result[y + padding, x + padding] = picture[y, x];

        return result;
    }
}
