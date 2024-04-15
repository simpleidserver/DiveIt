using System.Diagnostics.CodeAnalysis;

namespace CNN
{
    public class MatrixHelper
    {
        public static decimal Multiply(decimal[,] matrixA, decimal[,] matrixB)
        {
            var height = matrixA.GetLength(0);
            var width = matrixA.GetLength(1);
            decimal result = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    result += matrixA[y, x] * matrixB[y, x];
                }
            }

            return result;
        }

        public static decimal[,] Sum(decimal[,] matrixA, decimal[,] matrixB)
        {
            var height = matrixA.GetLength(0);
            var width = matrixA.GetLength(1);
            var result = new decimal[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    result[y,x] = matrixA[y, x] + matrixB[y, x];
                }
            }

            return result;
        }

        public static decimal[,] GetPortion(decimal[,] matrix, int width, int height, int startX, int startY)
        {
            var result = new decimal[height, width];
            for (var y = 0; y < result.GetLength(0); y++)
                for (var x = 0; x < result.GetLength(1); x++)
                {
                    result[y, x] = matrix[startY + y, startX + x];
                }

            return result;
        }

        public static decimal Max(decimal[,] matrix)
        {
            decimal result = default(decimal);
            for (var y = 0; y < matrix.GetLength(0); y++)
                for (var x = 0; x < matrix.GetLength(1); x++)
                {
                    var value = matrix[y, x];
                    if (value > result)
                        result = value;
                }

            return result;
        }

        public static decimal[,] Reshape(decimal[] data, int width, int height)
        {
            var result = new decimal[height, width];
            for(var y = 0; y < height; y++)
            {
                for(var x = 0; x < width; x++)
                {
                    var newIndex = (y * width) + x;
                    if (newIndex >= data.Length) return result;
                    result[y, x] = data[(y * width) + x];
                }
            }

            return result;
        }
    }
}
