using System.ComponentModel;

namespace SGDOptimization;

public static class MathUtils
{
    public static double[] InitializeVector(int size)
        => Enumerable.Repeat((double)0, size).ToArray();

    public static double[,] Transpose(double[,] matrix)
    {
        var result = new double[matrix.GetLength(1), matrix.GetLength(0)];
        for (var i = 0; i < matrix.GetLength(1); i++)
            for (var j = 0; j < matrix.GetLength(0); j++)
                result[i, j] = matrix[j, i];
        return result;
    }

    public static (double[,] a, double[] b) Shuffle(double[,] x, double[] y, int nbSamples, int nbWeight, int batchSize)
    {
        var rndIndexes = MathUtils.RandomNumbers(nbSamples, batchSize);
        var a = new double[batchSize, nbWeight];
        for (var i = 0; i < batchSize; i++)
        {
            var index = rndIndexes[i];
            for (var z = 0; z < nbWeight; z++)
                a[i, z] = x[index, z];
        }

        var b = rndIndexes.Select(i => y.ElementAt(i)).ToArray();
        return (a, b);
    }

    public static double[] Average(IEnumerable<double[]> vectors)
    {
        var result = new double[vectors.First().Count()];
        for (var i = 0; i < vectors.Count(); i++)
        {
            var vector = vectors.ElementAt(i);
            for (var j = 0; j < vector.Length; j++)
            {
                result[j] += vector[j];
            }
        }

        for (var t = 0; t < result.Length; t++)
        {
            result[t] = result[t] / vectors.Count();
        }

        return result;
    }

    public static double[] Multiply(double[,] matrix, double[] vector)
    {
        var result = new double[matrix.GetLength(0)];
        for (var i = 0; i < matrix.GetLength(0); i++)
        {
            double value = 0;
            for (var j = 0; j < matrix.GetLength(1); j++)
            {
                value += matrix[i, j] * vector[j];
            }

            result[i] = value;
        }

        return result;
    }

    public static double[,] Multiply(double[,] matrix, double value)
    {
        var result = new double[matrix.GetLength(0), matrix.GetLength(1)];
        for (var i = 0; i < matrix.GetLength(0); i++)
            for (var j = 0; j < matrix.GetLength(1); j++)
                result[i, j] = result[i, j] * value;

        return result;
    }

    public static double[] Substract(double[] vectorA, double[] vectorB)
    {
        var result = new double[vectorA.Length];
        for (var i = 0; i < vectorA.Length; i++)
            result[i] = vectorA[i] - vectorB[i];
        return result;
    }

    public static double[] Add(double[] vector, double value)
    {
        var result = new double[vector.Length];
        for (var i = 0; i < vector.Length; i++)
            result[i] += vector[i] + value;

        return result;
    }

    public static double[] Divide(double[] vector, double value)
    {
        var result = new double[vector.Length];
        for (var i = 0; i < vector.Length; i++)
            result[i] = vector[i] / value;

        return result;
    }

    public static double[] Multiply(double[] vector, double value)
    {
        var result = new double[vector.Length];
        for (var i = 0; i < vector.Length; i++)
            result[i] = vector[i] * value;

        return result;
    }

    private static List<int> RandomNumbers(int max, int batchSize)
    {
        var result = new List<int>();
        var rnd = new Random();
        for (var i = 0; i < batchSize; i++)
        {
            while (true)
            {
                var record = rnd.Next(0, max);
                if (result.Contains(record)) continue;
                result.Add(record);
                break;
            }

        }
        return result;
    }
}