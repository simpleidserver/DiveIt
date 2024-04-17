namespace CNN.Extensions
{
    public static class MultiDimArrayExtensions
    {
        public static double[,] TransformFirstRecordInto2DArray(this double[,,] arr)
        {
            var result = new double[arr.GetLength(1), arr.GetLength(2)];
            for(var y = 0; y < arr.GetLength(1); y++)
            {
                for(var x = 0; x < arr.GetLength(2); x++)
                {
                    result[y,x] = arr[0, y,x];
                }
            }

            return result;
        }
    }
}
