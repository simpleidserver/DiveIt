// See https://aka.ms/new-console-template for more information

// implémenter cross correlation.
// implémenter convolution.

using CNN;
using CNN.Extensions;
using CNN.Layers;

void TrainHandWritter()
{
    var rnd = new Random();
    var tt = rnd.NextDecimal(-1, 1);

    // Read data.
    var content = File.ReadAllLines("c:\\Projects\\DiveIt\\Projects\\CNN\\A_Z Handwritten Data.csv")
        .Select(s => s.Split(',').Select(n => decimal.Parse(n)));
    var trainSize = 0.2;
    var numberToTrain = (int)(content.Count() * trainSize);
    var trainData = content.Take(numberToTrain);
    var trainX = trainData.Select(s => MatrixHelper.Reshape(s.Skip(1).ToArray(), 28, 28)).ToArray();
    var trainY = trainData.Select(s => s.First()).ToArray();

    // train the model.
    var network = new Network();
    network.Layers.Add(new ConvolutionLayer(8, 3));
    network.Layers.Add(new MaxPoolingLayer(2, 2));
    network.Layers.Add(new SoftmaxLayer(10));
    network.Fit(trainX, trainY);

}

void TestMaxPooling()
{
    var data = new decimal[][,]
    {
        new decimal[,]
        {
            { 2, 2, 7, 3 },
            { 9, 4, 6, 1 },
            { 8, 5, 2, 4 },
            { 3, 1, 2, 6 }
        }
    };
    var network = new Network();
    network.Layers.Add(new MaxPoolingLayer(2, 2));
    network.Predict(data);
}

void TestSoftmaxLayer()
{
    var data = new decimal[][,]
    {
        new decimal[,]
        {
            { 2, 2, 7, 3 },
            { 9, 4, 6, 1 }
        }
    };
    var network = new Network();
    network.Layers.Add(new SoftmaxLayer(2));
    network.Predict(data);
}

TrainHandWritter();
// TestMaxPooling();
// TestSoftmaxLayer();
// ExtractGrayScale();
// Convulation();
// MaxPool();
// ReadData();

Console.WriteLine("Hello, World!");