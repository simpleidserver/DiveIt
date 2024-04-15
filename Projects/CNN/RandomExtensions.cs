using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CNN
{
    public static class RandomExtensions
    {
        public static decimal NextDecimal(this Random rng, int minValue, int maxValue)
        {
            var next = rng.NextDouble();
            return (decimal)(minValue + (next * (maxValue - minValue)));
        }
    }
}
