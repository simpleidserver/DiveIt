using System.Text.Json.Nodes;

namespace CNN.Layers
{
    public interface ILayer
    {
        double[,,] Forward(double[,,] matrix);
        double[,,] Backward(double[,,] matrix);
    }
}
