namespace CNN.Layers
{
    public interface ILayerNeuron
    {
        decimal[,] Forward(decimal[][,] matrixLst);
    }
}
