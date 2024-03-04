namespace SGD;

public class GradientDescentRegression
{
    public static void Compute(
        double learningRate,
        int epoch,
        double[,] x,
        double[] y,
        bool isStochastic = false,
        int batchSize = 0)
    {
        int nbSamples = x.GetLength(0);
        var nbWeight = x.GetLength(1);
        var w = MathUtils.InitializeVector(nbWeight);
        double itp = 0;
        var lst = new List<double>();
        for (var step = 0; step < epoch; step++)
        {
            var tmpX = x;
            var tmpY = y;
            if(isStochastic)
            {
                var shuffle = MathUtils.Shuffle(x, y, nbSamples, nbWeight, batchSize);
                tmpX = shuffle.a;
                tmpY = shuffle.b;
            }

            // 1. itp + w*txi
            var newY = CPUMathUtils.Add(
                CPUMathUtils.Multiply(tmpX, w),
                itp);
            // 2. losefactor = tyi - (itp + w*txi)
            var loseFactor = CPUMathUtils.Substract(tmpY, newY);
            // 3. η * (1/N Σ(i/n) -2 * wT*txi * losefactor
            var tmp = CPUMathUtils.Multiply(
                CPUMathUtils.Divide(
                    CPUMathUtils.Multiply(
                        CPUMathUtils.Multiply(
                            CPUMathUtils.Transpose(tmpX),
                            loseFactor
                        ),
                        -2
                    ),
                    nbSamples
                ),
                learningRate);
            // 4. Update weight
            w = CPUMathUtils.Substract(w, tmp);
            // 5. Update intercept : itp - η * (1/N Σ(i/n) -2 * (loseFactor))
            itp -= learningRate * (CPUMathUtils.Multiply(loseFactor, -2).Sum() / nbSamples);
            newY = CPUMathUtils.Add(CPUMathUtils.Multiply(tmpX, w), itp);
            lst.Add(ComputeMse(tmpY, newY));
        }

        for (var ind = 0; ind < epoch; ind++)
        {
            Console.WriteLine($"Iteration {ind + 1}, MSE is {lst.ElementAt(ind)}");
        }

        for(var ind = 0; ind < w.Count(); ind++)
        {
            Console.WriteLine($"Iteration {ind + 1}, Weight is {w.ElementAt(ind)}");
        }

        var endResult = CPUMathUtils.Add(CPUMathUtils.Multiply(x, w), itp);
        Console.WriteLine($"Intercept {itp}");
    }

    private static double ComputeMse(double[] vector, double[] computedVector)
    {
        var nbSamples = vector.Count();
        var result = Math.Pow(CPUMathUtils.Substract(vector, computedVector).Sum(), 2);
        return result / nbSamples;
    }
}
