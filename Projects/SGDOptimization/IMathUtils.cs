namespace SGDOptimization
{
    public interface IMathUtils
    {
        SgdResult Sgd(double[,] x, double[] y, double[] w, double itp);
    }

    public record SgdResult
    {
        public SgdResult(double[] weight, double itp)
        {
            Weight = weight;
            Itp = itp;
        }

        public double[] Weight { get; private set; }
        public double Itp { get; private set; }
    }
}
