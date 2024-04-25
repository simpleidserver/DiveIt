namespace CNN.Extensions
{
    public static class RandomExtensions
    {
        public static double NextDouble(this Random rng, int minValue, int maxValue)
        {
            var next = rng.NextDouble();
            return (minValue + next * (maxValue - minValue));
        }
    }
}
