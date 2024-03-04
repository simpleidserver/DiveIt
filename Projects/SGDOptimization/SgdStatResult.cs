namespace SGDOptimization
{
    public class SgdStatResult
    {
        public double Mse { get; set; }
        public Dictionary<int, long> ExecutionTimes {  get; set; }
    }
}
