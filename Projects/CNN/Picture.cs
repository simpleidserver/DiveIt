using Emgu.CV.Structure;
using Emgu.CV;
using System.Drawing;
using System;

namespace CNN
{
    public class Picture
    {
        public static double[] ExtractGrayScale(string path)
        {
            var r = CvInvoke.Imread(path, Emgu.CV.CvEnum.ImreadModes.Grayscale);
            var data = r.GetData() as byte[,];
            var result = new double[data.GetLength(0) * data.GetLength(1)];
            for(var y = 0; y < data.GetLength(0); y++)
                for(var x = 0; x < data.GetLength(1); x++)
                    result[(y * data.GetLength(1)) + x] = data[y, x];
            return result;
        }

        public static void Save(string path, double[,] result)
        {
            var matrix = new Emgu.CV.Matrix<double>(result);
            CvInvoke.Imwrite(path, matrix);
        }
    }
}