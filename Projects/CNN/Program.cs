// See https://aka.ms/new-console-template for more information

// implémenter cross correlation.
// implémenter convolution.

using CNN;
using CNN.Layers;

void TrainHandWritter()
{
    // CONV :
    // images = np.array([[0,1,2,3,4],[5,6,7,8,9],[10,11,12,14,15]])
    // array([[[-0.16695259, -0.19559588,  0.00095058],
    //  [ 0.0667812 , -0.01323354,  0.02633286],
    //  [-0.0693899 ,  0.02499117,  0.02315857]],
    // 
    // [[ 0.09233382,  0.01851037,  0.02374835],
    //  [-0.23014707, -0.18702714,  0.0176147 ],
    //  [-0.13144203,  0.09536866,  0.10891081]]])
    // output = array([[[ 0.10404682, -1.04202347],
    // [-0.17575214, -1.12524218],
    //    [-0.45371849, -1.22200303]]])

    // MAX POOL
    // images = np.array([[[0,1,2,4],[2,3,4,5]],[[6,7,8,9],[10,11,12,14]]])
    // output : array([[[10., 11., 12., 14.]]])

    // Read data.
    var content = File.ReadAllLines("c:\\Projects\\DiveIt\\Projects\\CNN\\A_Z Handwritten Data.csv")
        .Select(s => s.Split(',').Select(n => double.Parse(n)));
    var trainSize = 0.2;
    var numberToTrain = (int)(content.Count() * trainSize);
    var trainData = content.Take(numberToTrain);

    var trainX = trainData.Select(s => ArrayHelper.Reshape(s.Skip(1).Select(i => (i / 255) - 0.5).ToArray(), 28, 28)).ToArray();
    var trainY = trainData.Select(s => s.First()).ToArray();

    // train the model.
    var network = new Network();
    network.Layers.Add(new ConvolutionLayer(8, 3, 0));
    network.Layers.Add(new MaxPoolingLayer(2, 2));
    network.Layers.Add(new SoftmaxLayer(10));
    network.Train(trainX, trainY);

}

void TestMaxPooling()
{
    var data = new double[,,]
    {
        {
            { 0, 1, 2, 4 },
            { 2,3,4,5 }
        },
        {
            { 6,7,8,9 },
            { 10,11,12,14 }
        }
    };
    var network = new Network();
    network.Layers.Add(new MaxPoolingLayer(2, 2));
    network.Predict(data);
}

void TestConv()
{
    // [[0,1,2,3,4],[5,6,7,8,9],[10,11,12,14,15]]
    var data = new double[,,]
    {
        {
            { 0,1,2,3,4 },
            { 5,6,7,8,9 },
            { 10,11,12,14,15 }
        }
    };
    var network = new Network();
    var convol = new ConvolutionLayer(2, 3, 0);
    convol.Neurons.Clear();
    convol.Neurons.Add(new ConvolutionLayerNeuron
    {
        Weights = new double[,]
        {
            { -0.16695259, -0.19559588,  0.00095058 },
            { 0.0667812 , -0.01323354,  0.02633286 },
            { -0.0693899 ,  0.02499117,  0.02315857 }
        }
    });
    convol.Neurons.Add(new ConvolutionLayerNeuron
    {
        Weights = new double[,]
        {
            { 0.09233382,  0.01851037,  0.02374835 },
            { -0.23014707, -0.18702714,  0.0176147 },
            { -0.13144203,  0.09536866,  0.10891081 }
        }
    });
    network.Layers.Add(convol);
    network.Predict(data);
}

void TestSoftmaxLayer()
{
    var data = new double[,,]
    {
        {
            { 10, 11, 12, 14 }
        }
    };
    var network = new Network();
    network.Layers.Add(new SoftmaxLayer(2)
    {
        Weights = new double[,]
        {
            { 0.18738502, -0.0596138 },
            { -0.37713842,  0.16951041 },
            { 0.24996534,  0.18204466 },
            { 0.02242946, -0.0404026 }
        }
    });
    network.Predict(data);
}

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

// TestConv();
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