namespace CNN.Layers
{
    public interface ILayer
    {
        decimal[][,] Forward(decimal[][,] matrix);
    }
}
