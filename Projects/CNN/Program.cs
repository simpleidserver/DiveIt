// See https://aka.ms/new-console-template for more information

// implémenter cross correlation.
// implémenter convolution.

using CNN;
using CNN.Layers;

Network TrainModel()
{
    // The dataset must be downloaded from kaggle :https://www.kaggle.com/datasets/oddrationale/mnist-in-csv
    var content = File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "mnist_train.csv"))
        .Skip(1)
        .Select(s => s.Split(',').Select(n => double.Parse(n)));
    var trainData = content.Take(5000);
    var trainX = trainData.Select(s => ArrayHelper.Reshape(s.Skip(1).Select(i => (i / 255) - 0.5).ToArray(), 28, 28)).ToArray();
    var trainY = trainData.Select(s => s.First()).ToArray();

    var network = new Network();
    network.Layers.Add(new ConvolutionLayer(8, 3, padding: 0));
    network.Layers.Add(new MaxPoolingLayer(2, 2));
    network.Layers.Add(new SoftmaxLayer(10));
    network.Train(trainX, trainY, 3);
    return network;
}

void Predict(Network network)
{
    var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "4.png");
    var grayScale = Picture.ExtractGrayScale(imgPath);
    var grayScaleMatrix = ArrayHelper.Reshape(grayScale.Select(i => (i / 255) - 0.5).ToArray(), 28, 28);
    var result = network.Predict(grayScaleMatrix);
    var probs = new double[result.GetLength(2)];
    for (int i = 0; i < result.GetLength(2); i++)
        probs[i] = result[0, 0, i];
    var maxProb = probs.Max();
    Console.WriteLine($"Number '{probs.ToList().IndexOf(maxProb)}' is displayed in the image");
}

var trainedNetwork = TrainModel();
Predict(trainedNetwork);

Console.WriteLine("Hello, World!");