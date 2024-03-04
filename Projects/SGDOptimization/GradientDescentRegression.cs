using System.Collections.Concurrent;
using System.Diagnostics;

namespace SGDOptimization;

public class GradientDescentRegression
{
    private readonly IMathUtils _mathUtils;

    public GradientDescentRegression(IMathUtils mathUtils)
    {

        _mathUtils = mathUtils;
    }

    public SgdStatResult Compute(
        double learningRate,
        int epoch,
        double[,] x,
        double[] y,
        int batchSize = 0,
        int k = 2,
        bool isParallel = false)
    {
        var dic = new Dictionary<int, long>();
        int nbSamples = x.GetLength(0);
        var nbWeight = x.GetLength(1);
        var w = MathUtils.InitializeVector(nbWeight);
        double itp = 0;
        var sw = new Stopwatch();
        sw.Start();
        for (var step = 0; step < epoch; step++)
        {
            var concurrentWeight = new ConcurrentBag<double[]>();
            var concurrentItp = new ConcurrentBag<double>();
            // Suffle data and send parameters to worker nodes.
            var dataLst = Enumerable.Range(1, k).Select(i =>
            {
                return MathUtils.Shuffle(x, y, nbSamples, nbWeight, batchSize);
            });

            sw.Reset();
            sw.Start();
            var gradientWeightLst = new ConcurrentBag<double[]>();
            var gradientInterceptLst = new ConcurrentBag<double>();
            Console.WriteLine($"Iteration {step}");
            if(isParallel)
            {
                Parallel.ForEach(dataLst, shuffle =>
                {
                    var sgdResult = _mathUtils.Sgd(shuffle.a, shuffle.b, w, itp);
                    gradientWeightLst.Add(sgdResult.Weight);
                    gradientInterceptLst.Add(sgdResult.Itp);
                });
            }
            else
            {
                foreach(var shuffle in dataLst)
                {
                    var sgdResult = _mathUtils.Sgd(shuffle.a, shuffle.b, w, itp);
                    gradientWeightLst.Add(sgdResult.Weight);
                    gradientInterceptLst.Add(sgdResult.Itp);
                }
            }

            sw.Stop();
            dic.Add(step, sw.ElapsedMilliseconds);

            // Compute the average.
            var avgGradientWeight = MathUtils.Average(gradientWeightLst.ToArray());
            var avgItp = gradientInterceptLst.Sum() / gradientInterceptLst.Count();

            // Update weight and itp.
            var tmp = MathUtils.Multiply(MathUtils.Divide(
                    avgGradientWeight,
                    (double)nbSamples
                ), learningRate);
            w = MathUtils.Substract(w, tmp);
            itp -= learningRate * (avgItp / nbSamples);
        }

        var newY = MathUtils.Add(MathUtils.Multiply(x, w), itp);
        var mse = ComputeMse(y, newY);
        return new SgdStatResult
        {
            Mse = mse,
            ExecutionTimes = dic
        };
    }

    private double ComputeMse(double[] vector, double[] computedVector)
    {
        var nbSamples = vector.Count();
        var result = Math.Pow(MathUtils.Substract(vector, computedVector).Sum(), 2);
        return result / nbSamples;
    }
}
