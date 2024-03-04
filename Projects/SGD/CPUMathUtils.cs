namespace SGD;

public class CPUMathUtils
{
    public static double[] Multiply(double[,] matrix, double[] vector) 
    {
        var result = new double[matrix.GetLength(0)];
        for(var i = 0; i < matrix.GetLength(0); i++)
        {
            double value = 0;
            for(var j = 0; j < matrix.GetLength(1); j++)
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
        for(var i = 0; i < matrix.GetLength(0); i++)
            for(var j = 0; j < matrix.GetLength(1); j++)
                result[i,j] = result[i,j] * value;
        
        return result;
    }

    public static double[,] Divide(double[,] matrix, double value)
    {
        var result = new double[matrix.GetLength(0), matrix.GetLength(1)];
        for(var i = 0; i < matrix.GetLength(0); i++)
            for(var j = 0; j < matrix.GetLength(1); j++)
                result[i,j] = result[i,j] / value;
        
        return result;
    }

    public static double[] Substract(double[] vectorA, double[] vectorB)
    {
        var result = new double[vectorA.Length];
        for (var i = 0; i < vectorA.Length; i++)
            result[i] = vectorA[i] - vectorB[i];
        return result;
    }

    public static double[] Multiply(double[] vector, double value)
    {
        var result = new double[vector.Length];
        for (var i = 0; i < vector.Length; i++)
            result[i] = vector[i] * value;

        return result;
    }

    public static double[] Divide(double[] vector, double value)
    {
        var result = new double[vector.Length];
        for (var i = 0; i < vector.Length; i++)
            result[i] = vector[i] / value;

        return result;
    }

    public static double[] Add(double[] vector, double value) 
    {
        var result = new double[vector.Length];
        for(var i = 0; i < vector.Length; i++)
            result[i] += vector[i] + value;

        return result;
    }

    public static double[,] Transpose(double[,] matrix)
    {
        var result = new double[matrix.GetLength(1), matrix.GetLength(0)];
        for(var i = 0; i < matrix.GetLength(1); i++)
            for(var j = 0; j < matrix.GetLength(0); j++)
                result[i,j] = matrix[j,i];
        return result;
    }
}