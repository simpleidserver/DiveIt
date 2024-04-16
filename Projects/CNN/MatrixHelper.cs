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

        public static decimal[] Flat(decimal[][,] matrixLst)
        {
            var result = new decimal[matrixLst.Count() * matrixLst[0].GetLength(0) * matrixLst[0].GetLength(1)];
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

        public static decimal[] Multiply(decimal[] vector, List<decimal[]> matrix)
        {
            var result = new decimal[matrix.Count()];
            for(var i = 0; i < matrix.Count(); i++)
            {
                var matrixVector = matrix[i];
                decimal val = 0;
                for(var y = 0; y < matrixVector.Count(); y++)
                {
                    val += matrixVector[y] * vector[i];
                }

                result[i] = val;
            }

            return result;
        }

        public static decimal[] Sum(decimal[] firstVector, decimal[] secondVector)
        {
            var result = new decimal[firstVector.Count()];
            for (var i = 0; i < firstVector.Count(); i++)
                result[i] = firstVector[i] + secondVector[i];

            return result;
        }

        public static decimal[] Exp(decimal[] vector)
        {
            var result = new decimal[vector.Length];
            for (var i = 0; i < vector.Count(); i++)
                result[i] = (decimal)Math.Exp((double)vector[i]);
            return result;
        }

        public static decimal[] Divide(decimal[] vector,  decimal divisor)
        {
            var result = new decimal[vector.Length];
            for (var i = 0; i < vector.Count(); i++)
                result[i] = vector[i] / divisor;
            return result;
        }
    }
}
