// See https://aka.ms/new-console-template for more information

// implémenter cross correlation.
// implémenter convolution.

using CNN;
using CNN.Layers;

void TrainHandWritter()
{
    // Read data.
    var content = File.ReadAllLines("c:\\Projects\\DiveIt\\Projects\\CNN\\A_Z Handwritten Data.csv")
        .Select(s => s.Split(',').Select(n => double.Parse(n)));
    var trainSize = 0.2;
    var numberToTrain = (int)(content.Count() * trainSize);
    var trainData = content.Take(numberToTrain);
    var trainX = trainData.Select(s => ArrayHelper.Reshape(s.Skip(1).ToArray(), 28, 28)).ToArray();
    var trainY = trainData.Select(s => s.First()).ToArray();

    // train the model.
    var network = new Network();
    network.Layers.Add(new ConvolutionLayer(8, 3));
    network.Layers.Add(new MaxPoolingLayer(2, 2));
    network.Layers.Add(new SoftmaxLayer(10));
    network.Train(trainX, trainY);

}

void TestMaxPooling()
{
    var data = new double[,,]
    {
        {
            { 2, 2, 7, 3 },
            { 9, 4, 6, 1 }
        },
        {
            { 9, 4, 6, 1 },
            { 9, 4, 6, 1 }
        },
        {
            { 8, 5, 2, 4 },
            { 9, 4, 6, 1 }
        },
        {
            { 3, 1, 2, 6 },
            { 9, 4, 6, 1 }
        }
    };
    var network = new Network();
    network.Layers.Add(new MaxPoolingLayer(2, 2));
    network.Predict(data);
}

/*
void TestSoftmaxLayer()
{
    var data = new double[][,]
    {
        new double[,]
        {
            { 2, 2, 7, 3 },
            { 9, 4, 6, 1 }
        }
    };
    var network = new Network();
    network.Layers.Add(new SoftmaxLayer(2));
    network.Predict(data);
}
*/

void Dot()
{
    var vector = new double[]
    {
        0.5, 1
    };
    var matrix = new double[,]
    {
        { 0, 1, 3, 4 },
        { 2, 3, 3, 4 }
    };
    var r = ArrayHelper.Multiply(vector, matrix);
    string sss = "";
}

void TransposeAndMultiplyWithNewAxis()
{
    var firstVector = new double[]
    {
        0.2, 0.3
    };
    var secondVector = new double[]
    {
        1, 2
    };
    var r = ArrayHelper.TransposeAndMultiplyWithNewAxis(firstVector, secondVector);
    string ss = "";
}

void MultiplyAndSum()
{
    var matrix = new double[,]
    {
        { 1, 2, 3 },
        { 3, 4, 5 }
    };
    var vector = new double[]
    {
        1, 2, 3
    };
    var r = ArrayHelper.MultiplyAndSum(matrix, vector);
    string ss = "";
}

/*
void Reshape()
{
    var l = new double[]
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9
    };
    var t =  ArrayHelper.Reshape(l, 2, 2, 1);
    string ss = "";
}
*/

// TestMaxPooling();
// Transpose();
// Reshape();
TrainHandWritter();
// TestMaxPooling();
// TestSoftmaxLayer();
// ExtractGrayScale();
// Convulation();
// MaxPool();
// ReadData();

Console.WriteLine("Hello, World!");