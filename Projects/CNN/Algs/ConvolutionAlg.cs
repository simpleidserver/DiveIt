namespace CNN.Algs;

public class ConvolutionAlg
{
    public static decimal[,] Transform(decimal[,] picture, decimal[,] filter, int padding = 1, int strideW = 1, int strideH = 1)
    {
        var resizedPicture = ResizePicture(picture, padding);
        var outputShape = GetOutputShape(picture, filter, padding, strideW, strideH);
        var result = new decimal[
            outputShape.height,
            outputShape.width
        ];
        for (var y = 0; y < result.GetLength(0); y++)
        {
            for (var x = 0; x < result.GetLength(1); x++)
            {
                var portion = MatrixHelper.GetPortion(
                    resizedPicture,
                    filter.GetLength(1),
                    filter.GetLength(0),
                    x * strideW,
                    y * strideH);
                result[y, x] = MatrixHelper.Multiply(portion, filter);
            }
        }

        return result;
    }

    public static (int height, int width) GetOutputShape(decimal[,] picture, decimal[,] filter, int padding = 1, int strideW = 1, int strideH = 1)
    {
        return (height: (picture.GetLength(0) - filter.GetLength(0) + padding * 2 + strideH) / strideH,
            width: (picture.GetLength(1) - filter.GetLength(1) + padding * 2 + strideW) / strideW);
    }

    public static decimal[,] ResizePicture(decimal[,] picture, int padding)
    {
        var newHeight = picture.GetLength(0) + padding * 2;
        var newWidth = picture.GetLength(1) + padding * 2;
        var result = new decimal[newHeight, newWidth];
        for (var y = 0; y < picture.GetLength(0); y++)
            for (var x = 0; x < picture.GetLength(1); x++)
                result[y + padding, x + padding] = picture[y, x];

        return result;
    }
}
