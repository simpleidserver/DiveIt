namespace CNN.Layers
{
    public interface ILayerNeuron
    {
        double[,] Forward(double[][,] matrixLst);
    }
}
