namespace CNN.Layers
{
    public class BaseLayer<T> : ILayer where T : BaseLayerNeuron
    {
        public List<T> Neurons = new List<T>();

        public decimal[][,] Forward(decimal[][,] matrixLst)
        {
            var result = new decimal[Neurons.Count][,];
            for (var i = 0; i < Neurons.Count(); i++)
                result[i] = Neurons[i].Forward(matrixLst);
            return result;
        }
    }
}
