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
    var trainSize = 0.2;
    var numberToTrain = (int)(content.Count() * trainSize);
    var trainData = content.Skip(numberToTrain);
    var trainX = trainData.Select(s => ArrayHelper.Reshape(s.Skip(1).Select(i => (i / 255) - 0.5).ToArray(), 28, 28)).ToArray();
    var trainY = trainData.Select(s => s.First()).ToArray();

    var network = new Network();
    network.Layers.Add(new ConvolutionLayer(8, 3, 0));
    network.Layers.Add(new MaxPoolingLayer(2, 2));
    network.Layers.Add(new SoftmaxLayer(10));
    network.Train(trainX, trainY);
    return network;
}

void Predict(Network network)
{
    var imgPath = Path.Combine(Directory.GetCurrentDirectory(), "1.png");
    var grayScaleMatrix = Picture.ExtractGrayScale(imgPath);
    var result = network.Predict(ArrayHelper.TransformTo3D(grayScaleMatrix));
    string ss = "";
}

void Test()
{
    var images = new double[,,]
    {
        {
            { 0, 1, 2, 4 },
            { 2, 3, 4, 5 }
        },
        {
            { 6, 7, 8, 9 },
            { 10, 11, 12, 14 }
        }
    };
    var p = new double[,,]
    {
        {
            { 1, 1, 1, 1, 1 }
        }
    };
    var l = new MaxPoolingLayer(2, 2);
    l.Forward(images);
    var t = l.Backward(p);
    string ss = "";
}

var trainedNetwork = TrainModel();
Predict(trainedNetwork);
// Test();
Console.WriteLine("Hello, World!");