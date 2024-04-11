using System.Drawing;

namespace CNN;

public class Picture
{
    public static ImageExtractionResult Extract(string path)
    {
        var img = new Bitmap(path);
        var redMatrix = new int[img.Height,img.Width];
        var greenMatrix = new int[img.Height, img.Width];
        var blueMatrix = new int[img.Height, img.Width];
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

    public static void Save(string path, ImageExtractionResult result)
    {
        var height = result.BlueMatrix.GetLength(0);
        var width = result.BlueMatrix.GetLength(1);
        var img = new Bitmap(width, height);
        for(var y = 0; y < height; y++)
        {
            for(var x = 0; x < width; x++)
            {
                img.SetPixel(x, y, Color.FromArgb(
                    result.RedMatrix[y,x], 
                    result.GreenMatrix[y,x], 
                    result.BlueMatrix[y, x]));
            }
        }

        img.Save(path);
    }

    public record ImageExtractionResult
    {
        public int[,] GreenMatrix { get; set; }
        public int[,] RedMatrix { get; set; }
        public int[,] BlueMatrix { get; set; }
    }
}
