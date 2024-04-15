using Emgu.CV;
using System.Drawing;

namespace CNN;

public class Picture
{
    public static ImageExtractionResult Extract(string path)
    {
        var img = new Bitmap(path);
        var redMatrix = new decimal[img.Height,img.Width];
        var greenMatrix = new decimal[img.Height, img.Width];
        var blueMatrix = new decimal[img.Height, img.Width];
        for(var y = 0; y < img.Height; y++)
        {
            for(var x = 0; x < img.Width; x++)
            {
                var pixel = img.GetPixel(x, y);
                redMatrix[y, x] = pixel.R;
                greenMatrix[y, x] = pixel.G;
                blueMatrix[y, x] = pixel.B;
            }
        }

        return new ImageExtractionResult
        {
            BlueMatrix = blueMatrix,
            GreenMatrix = greenMatrix,
            RedMatrix = redMatrix
        };
    }

    public static decimal[,] ExtractGrayScale(string path)
    {
        var img = new Bitmap(path);
        var result = new decimal[img.Height, img.Width];
        for (var y = 0; y < img.Height; y++)
        {
            for (var x = 0; x < img.Width; x++)
            {
                var pixel = img.GetPixel(x, y);
                decimal grayScale = (decimal)((pixel.R * .3) + 
                    (pixel.G * .59) + 
                    (pixel.B * .11));
                result[y, x] = grayScale;
            }
        }

        return result;
    }

    public static void Save(string path, ImageExtractionResult result)
    {
        // var matrix = new Emgu.CV.Matrix<int>(result.BlueMatrix);
        // CvInvoke.Imwrite(path, matrix);
        /*
        var height = result.BlueMatrix.GetLength(0);
        var width = result.BlueMatrix.GetLength(1);
        var img = new Bitmap(width, height);
        for(var y = 0; y < height; y++)
        {
            for(var x = 0; x < width; x++)
            {
                img.SetPixel(x, y, Color.FromArgb(
                    GetPixel(result.RedMatrix[y,x]), 
                    GetPixel(result.GreenMatrix[y,x]), 
                    GetPixel(result.BlueMatrix[y, x])));
            }
        }

        img.Save(path);

        int GetPixel(int val) => val < 0 ? 0 : (val > 255 ? 255 : val);
        */
    }

    public record ImageExtractionResult
    {
        public decimal[,] GreenMatrix { get; set; }
        public decimal[,] RedMatrix { get; set; }
        public decimal[,] BlueMatrix { get; set; }
    }
}
