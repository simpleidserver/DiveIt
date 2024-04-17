﻿namespace CNN.Layers
{
    public interface ILayer
    {
        double[,,] Forward(double[,,] matrix);
        double[,,] Backward(double[,,] matrix);
    }
}
