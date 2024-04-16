namespace CNN
{
    public class MatrixHelper
    {
        public static double Multiply(double[,] matrixA, double[,] matrixB)
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
            {
                for (var x = 0; x < width; x++)
                {
                    result[y,x] = matrixA[y, x] + matrixB[y, x];
                }
            }

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

        public static double Max(double[,] matrix)
        {
            double result = default(double);
            for (var y = 0; y < matrix.GetLength(0); y++)
                for (var x = 0; x < matrix.GetLength(1); x++)
                {
                    var value = matrix[y, x];
                    if (value > result)
                        result = value;
                }

            return result;
        }

        public static double[,] Reshape(double[] data, int width, int height)
        {
            var result = new double[height, width];
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

        public static double[] Flat(double[][,] matrixLst)
        {
            var result = new double[matrixLst.Count() * matrixLst[0].GetLength(0) * matrixLst[0].GetLength(1)];
            for(var m = 0; m < matrixLst.Count(); m++)
            {
                int i = 0;
                var matrix = matrixLst[m];
                for(var y = 0; y < matrix.GetLength(0); y++)
                {
                    for(var x = 0; x < matrix.GetLength(1); x++)
                    {
                        result[i] = matrix[y, x];
                        i++;
                    }
                }
            }

            return result;
        }

        public static double[] Multiply(double[] vector, List<double[]> matrix)
        {
            var result = new double[matrix.Count()];
            for(var i = 0; i < matrix.Count(); i++)
            {
                var matrixVector = matrix[i];
                double val = 0;
                for(var y = 0; y < matrixVector.Count(); y++)
                {
                    val += matrixVector[y] * vector[i];
                }

                result[i] = val;
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
    }
}
