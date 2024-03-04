namespace SGD;

public static class MathUtils
{
    public static double[] InitializeVector(int size)
        => Enumerable.Repeat((double)0, size).ToArray();

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