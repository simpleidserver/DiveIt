using CNN.Layers;

namespace CNN
{
    public class Network
    {
        public List<ILayer> Layers = new List<ILayer>();

        public void Fit(
            decimal[][,] trainX, 
            decimal[] trainY)
        {
            for(var i = 0; i < trainX.Count(); i++)
            {
                var trainImage = trainX[i];
                var label = (int)trainY[i];
                var output = new decimal[1][,]
                {
                    trainImage
                };
                foreach (var layer in Layers)
                    output = layer.Forward(output);
                // calculate cross-entropy loss:
                var probs = output.Select(s => s[0, 0]).ToList();
                var crossEntropyLoss = -Math.Log((double)probs.ElementAt(label));
                bool isAccurate = false;
                var predictedLabel = probs.IndexOf(probs.Max());
                if(predictedLabel == label)
                {

                }
            }
        }

        public void Predict(decimal[][,] dataLst)
        {
            for(var i = 0; i < dataLst.Length; i++)
            {
                var output = new decimal[1][,]
                {
                    dataLst[i]
                };
                foreach (var layer in Layers)
                    output = layer.Forward(output);
            }
        }
    }
}
