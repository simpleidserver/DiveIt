namespace SGDOptimization;

public class CPUMathUtils : IMathUtils
{
    public SgdResult Sgd(double[,] x, double[] y, double[] w, double itp)
    {
        // 1. itp + w*txi
        var newY = MathUtils.Add(
            MathUtils.Multiply(x, w),
            itp);
        // 2. losefactor = tyi - (itp + w*txi)
        var loseFactor = MathUtils.Substract(y, newY);
        // 3. Store the gradient of the weight : ?(i/n) -2 * wT*txi * losefactor
        var weightLst = MathUtils.Multiply(
                MathUtils.Multiply(
                    MathUtils.Transpose(x),
                    loseFactor
                ),
                -2
        );
        // 4. Store the gradient of the intercept : ?(i/n) -2 * (loseFactor))
        var newItp = MathUtils.Multiply(loseFactor, -2).Sum();
        return new SgdResult(weightLst, newItp);
    }
}