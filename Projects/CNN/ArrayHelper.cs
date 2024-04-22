namespace CNN
{
    public class ArrayHelper
    {
        #region Reshape

        public static double[,] TransformFirstRecordInto2DArray(double[,,] arr)
        {
            var result = new double[arr.GetLength(1), arr.GetLength(2)];
            for (var y = 0; y < arr.GetLength(1); y++)
            {
                for (var x = 0; x < arr.GetLength(2); x++)
                {
                    result[y, x] = arr[0, y, x];
                }
            }

            return result;
        }

        public static double[,,] TransformTo3D(double[] arr)
        {
            var result = new double[1, 1, arr.Length];
            for (var x = 0; x < arr.Length; x++)
                result[0, 0, x] = arr[x];
            return result;
        }

        public static double[,,] TransformTo3D(double[] vector, int nb, int width, int height)
        {
            var result = new double[nb, height, width];
            for (var n = 0; n < nb; n++)
            {
                var skip = n * width * height;
                for (var y = 0; y < height; y++)
                {
                    for (var x = 0; x < width; x++)
                    {
                        result[n, y, x] = vector[x + (y * width) + skip];
                    }
                }
            }

            return result;
        }

        public static double[,,] TransformTo3D(double[,] arr)
        {
            var result = new double[1, arr.GetLength(0), arr.GetLength(1)];
            for(var y = 0; y < arr.GetLength(0); y++)
                for(var x = 0; x < arr.GetLength(1); x++)
                    result[0, y, x] = arr[y, x];
            return result;
        }

        public static double[,] Reshape(double[] data, int width, int height)
        {
            var result = new double[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var newIndex = (y * width) + x;
                    if (newIndex >= data.Length) return result;
                    result[y, x] = data[(y * width) + x];
                }
            }

            return result;
        }

        public static double[] Flat(double[,,] matrix)
        {
            var result = new double[matrix.GetLength(0) * matrix.GetLength(1) * matrix.GetLength(2)];
            int i = 0;
            for (var m = 0; m < matrix.GetLength(0); m++)
            {
                for (var y = 0; y < matrix.GetLength(1); y++)
                {
                    for (var x = 0; x < matrix.GetLength(2); x++)
                    {
                        result[i] = matrix[m, y, x];
                        i++;
                    }
                }
            }

            return result;
        }

        #endregion

        #region Manipulate 1D array

        public static double[,] TransposeAndMultiplyWithNewAxis(double[] firstVector, double[] secondVector)
        {
            var result = new double[firstVector.Length, secondVector.Length];
            for (var y = 0; y < firstVector.Length; y++)
            {
                for (var x = 0; x < secondVector.Length; x++)
                {
                    result[y, x] = firstVector[y] * secondVector[x];
                }
            }

            return result;
        }

        public static double[] Multiply(double[] vector, double[,] matrix)
        {
            var result = new double[matrix.GetLength(1)];
            for (var x = 0; x < matrix.GetLength(1); x++)
            {
                double val = 0;
                for (var y = 0; y < matrix.GetLength(0); y++)
                {
                    val += matrix[y, x] * vector[y];
                }

                result[x] = val;
            }

            return result;
        }

        public static double[] Sum(double[] firstVector, double[] secondVector)
        {
            var result = new double[firstVector.Count()];
            for (var i = 0; i < firstVector.Count(); i++)
                result[i] = firstVector[i] + secondVector[i];

            return result;
        }

        public static double[] Substract(double[] firstVector, double[] secondVector)
        {
            var result = new double[firstVector.Count()];
            for (var i = 0; i < firstVector.Count(); i++)
                result[i] = firstVector[i] - secondVector[i];

            return result;
        }

        public static double[] Exp(double[] vector)
        {
            var result = new double[vector.Length];
            for (var i = 0; i < vector.Count(); i++)
                result[i] = Math.Exp(vector[i]);
            return result;
        }

        public static double[] Divide(double[] vector, double divisor)
        {
            var result = new double[vector.Length];
            for (var i = 0; i < vector.Count(); i++)
                result[i] = vector[i] / divisor;
            return result;
        }

        public static double[] Multiply(double[] vector, double m)
        {
            var result = new double[vector.Length];
            for (var i = 0; i < vector.Count(); i++)
                result[i] = vector[i] * m;
            return result;
        }

        #endregion

        #region Manipulate 2D array

        public static double Conv(double[,] matrixA, double[,] matrixB)
        {
            var height = matrixA.GetLength(0);
            var width = matrixA.GetLength(1);
            double result = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    result += matrixA[y, x] * matrixB[y, x];
                }
            }

            return result;
        }

        public static double[,] Sum(double[,] matrixA, double[,] matrixB)
        {
            var height = matrixA.GetLength(0);
            var width = matrixA.GetLength(1);
            var result = new double[height, width];
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    result[y, x] = matrixA[y, x] + matrixB[y, x];

            return result;
        }

        public static double[,] Substract(double[,] matrixA, double[,] matrixB)
        {
            var height = matrixA.GetLength(0);
            var width = matrixA.GetLength(1);
            var result = new double[height, width];
            for (var y = 0; y < height; y++)
                for (var x = 0; x < width; x++)
                    result[y, x] = matrixA[y, x] - matrixB[y, x];

            return result;
        }

        public static double[] MultiplyAndSum(double[,] matrix, double[] vector)
        {
            var result = new double[matrix.GetLength(0)];
            for(var y = 0; y < matrix.GetLength(0); y++)
            {
                for(var x = 0; x < matrix.GetLength(1); x++)
                {
                    result[y] += matrix[y, x] * vector[x];
                }
            }

            return result;
        }

        public static double[,] Multiply(double[,] matrix, double value)
        {
            var result = new double[matrix.GetLength(0), matrix.GetLength(1)];
            for (var y = 0; y < matrix.GetLength(0); y++)
                for (var x = 0; x < matrix.GetLength(1); x++)
                    result[y,x] = matrix[y,x] * value;

            return result;
        }

        public static double[,] GetPortion(double[,] matrix, int width, int height, int startX, int startY)
        {
            var result = new double[height, width];
            for (var y = 0; y < result.GetLength(0); y++)
                for (var x = 0; x < result.GetLength(1); x++)
                {
                    result[y, x] = matrix[startY + y, startX + x];
                }

            return result;
        }

        public static double[] Max(double[,,] arr)
        {
            var indexes = MaxIndexes(arr);
            var result = new double[arr.GetLength(2)];
            for (var x = 0; x < arr.GetLength(2); x++)
                result[x] = arr[indexes.h, indexes.y, x];
            return result;
        }

        public static (int h, int y) MaxIndexes(double[,,] arr)
        {
            (int h, int y) r = new (0, 0);
            double max = 0;
            for (var h = 0; h < arr.GetLength(0); h++)
            {
                for (var y = 0; y < arr.GetLength(1); y++)
                {
                    var values = new double[arr.GetLength(2)];
                    for (var x = 0; x < arr.GetLength(2); x++)
                        values[x] = arr[h, y, x];
                    var newSum = values.Sum();
                    if (newSum > max)
                    {
                        max = newSum;
                        r.h = h;
                        r.y = y;
                    }
                }
            }

            return r;
        }

        #endregion

        #region Manipulate 3D array

        public static double[,,] GetPortion(double[,,] matrix, int width, int height, int startX, int startY)
        {
            var result = new double[height, width, matrix.GetLength(2)];
            for (var y = 0; y < result.GetLength(0); y++)
                for (var x = 0; x < result.GetLength(1); x++)
                {
                    for (var c = 0; c < result.GetLength(2); c++)
                    {
                        result[y, x, c] = matrix[startY + y, startX + x, c];
                    }

                }

            return result;
        }

        #endregion
    }
}
