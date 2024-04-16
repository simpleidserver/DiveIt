namespace CNN.Layers
{
    public class BaseLayer<T> : ILayer where T : BaseLayerNeuron
    {
        public List<T> Neurons = new List<T>();

        public double[][,] Forward(double[][,] matrixLst)
        {
            var result = new double[Neurons.Count][,];
            for (var i = 0; i < Neurons.Count(); i++)
                result[i] = Neurons[i].Forward(matrixLst);
            return result;
        }
    }
}
