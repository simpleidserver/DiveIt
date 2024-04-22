using System.Drawing;

namespace CNN
{
    public class Picture
    {
        public static double[,] ExtractGrayScale(string path)
        {
            var img = new Bitmap(path);
            var result = new double[img.Height, img.Width];
            for (var y = 0; y < img.Height; y++)
            {
                for (var x = 0; x < img.Width; x++)
                {
                    var pixel = img.GetPixel(x, y);
                    double grayScale = ((pixel.R * .3) +
                        (pixel.G * .59) +
                        (pixel.B * .11));
                    result[y, x] = grayScale;
                }
            }

            return result;
        }
    }
}
