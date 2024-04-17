using CNN.Layers;

namespace CNN
{
    public class Network
    {
        public List<ILayer> Layers = new List<ILayer>();

        private NetworkForwardResult Forward(double[,] data, int label)
        {
            var output = ArrayHelper.TransformTo3D(data);
            foreach (var layer in Layers)
                output = layer.Forward(output);

            // Calculate the cross-entropy loss.
            var probs = new double[output.GetLength(2)];
            for (int i = 0; i < output.GetLength(2); i++)
                probs[i] = output[0, 0, i];
            var probLst = probs.ToList();
            var crossEntropyLoss = -Math.Log(probs.ElementAt(label));
            var predictedLabel = probLst.IndexOf(probs.Max());
            var result = new NetworkForwardResult { Output = probs.ToArray(), Loss = crossEntropyLoss };
            if (predictedLabel == label)
                result.IsCorrect = true;
            return result;
        }

        private NetworkForwardResult Train(
            double[,] data, 
            int label)
        {
            var forwardResult = Forward(data, label);
            var gradient = new double[forwardResult.Output.Length];
            gradient[label] = -1 / forwardResult.Output[label];
            var grads = ArrayHelper.TransformTo3D(gradient);
            var lastLayer = Layers.Last();
            lastLayer.Backward(grads);

            /*
            for(var i = Layers.Count - 1; i >= 0; i--)
            {
                var layer = Layers[i];
                layer.Backward(grads);
            }
            */

            return forwardResult;
        }

        public void Train(
            double[][,] trainX,
            double[] trainY)
        {
            double loss = 0;
            int nbCorrect = 0;
            for(var i = 0; i < trainX.Count(); i++)
            {
                var trainData = trainX[i];
                var label = (int)trainY[i];
                var trainResult = Train(trainData, label);
                if (i % 100 == 99)
                {
                    Console.WriteLine($"[Step {i}] Average loss : {loss / 100}, Accuracy: {nbCorrect}%");
                    loss = 0;
                    nbCorrect = 0;
                }

                loss += trainResult.Loss;
                if (trainResult.IsCorrect)
                    nbCorrect++;
            }
        }

        public void Predict(double[,,] dataLst)
        {
            var output = dataLst;
            foreach (var layer in Layers)
                output = layer.Forward(output);
        }

        public class NetworkForwardResult
        {
            public bool IsCorrect { get; set; }
            public double[] Output { get; set; }
            public double Loss { get; set; }
        }
    }
}
