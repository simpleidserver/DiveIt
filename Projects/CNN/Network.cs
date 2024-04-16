using CNN.Layers;

namespace CNN
{
    public class Network
    {
        public List<ILayer> Layers = new List<ILayer>();

        private NetworkForwardResult Forward(double[,] data, int label)
        {
            var output = new double[1][,]
            {
                data
            };
            foreach (var layer in Layers)
                output = layer.Forward(output);

            // Calculate the cross-entropy loss.
            var probs = output.Select(s => s[0, 0]).ToList();
            var crossEntropyLoss = -Math.Log(probs.ElementAt(label));
            var predictedLabel = probs.IndexOf(probs.Max());
            var result = new NetworkForwardResult { Output = probs.ToArray(), Loss = crossEntropyLoss };
            if (predictedLabel == label)
                result.IsCorrect = true;
            return result;
        }

        private void Train(
            double[,] data, 
            int label)
        {
            var forwardResult = Forward(data, label);
            var gradient = new double[forwardResult.Output.Length];
            gradient[label] = -1 / forwardResult.Output[label];
            // TODO
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
                Train(trainData, label);

                /*
                var output = new double[1][,]
                {
                    trainImage
                };
                foreach (var layer in Layers)
                    output = layer.Forward(output);

                // calculate cross-entropy loss:
                var probs = output.Select(s => s[0, 0]).ToList();
                var crossEntropyLoss = -Math.Log(probs.ElementAt(label));
                var predictedLabel = probs.IndexOf(probs.Max());
                loss += crossEntropyLoss;
                if (predictedLabel == label)
                    nbCorrect++;

                if(i % 100 == 99)
                {
                    Console.WriteLine($"[Step {i}] Average loss : {loss / 100}, Accuracy: {nbCorrect}%");
                    loss = 0;
                    nbCorrect = 0;
                }
                */
            }
        }

        public void Predict(double[][,] dataLst)
        {
            for(var i = 0; i < dataLst.Length; i++)
            {
                var output = new double[1][,]
                {
                    dataLst[i]
                };
                foreach (var layer in Layers)
                    output = layer.Forward(output);
            }
        }

        public class NetworkForwardResult
        {
            public bool IsCorrect { get; set; }
            public double[] Output { get; set; }
            public double Loss { get; set; }
        }
    }
}
