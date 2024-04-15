﻿using System.Diagnostics.CodeAnalysis;

namespace CNN
{
    public class MatrixHelper
    {
        public static int Multiply(int[,] matrixA, int[,] matrixB)
        {
            var height = matrixA.GetLength(0);
            var width = matrixA.GetLength(1);
            int result = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    result += matrixA[y, x] * matrixB[y, x];
                }
            }

            return result;
        }

        public static int[,] Sum(int[,] matrixA, int[,] matrixB)
        {
            var height = matrixA.GetLength(0);
            var width = matrixA.GetLength(1);
            var result = new int[height, width];
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    result[y,x] = matrixA[y, x] + matrixB[y, x];
                }
            }

            return result;
        }

        public static int[,] GetPortion(int[,] matrix, int width, int height, int startX, int startY)
        {
            var result = new int[height, width];
            for (var y = 0; y < result.GetLength(0); y++)
                for (var x = 0; x < result.GetLength(1); x++)
                {
                    result[y, x] = matrix[startY + y, startX + x];
                }

            return result;
        }

        public static int Max(int[,] matrix)
        {
            int result = default(int);
            for (var y = 0; y < matrix.GetLength(0); y++)
                for (var x = 0; x < matrix.GetLength(1); x++)
                {
                    var value = matrix[y, x];
                    if (value > result)
                        result = value;
                }

            return result;
        }

        public static int[,] Reshape(int[] data, int width, int height)
        {
            var result = new int[height, width];
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
