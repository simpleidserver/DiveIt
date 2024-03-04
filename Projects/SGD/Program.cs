// See https://aka.ms/new-console-template for more information


using SGD;

int epoch = 1000;
double learningRate = 0.05;
var x = new double[,]
{
    { 1 },
    { 2 },
    { 3 },
    { 4 },
    { 5 }
};
var y = new double[]
{
    1,
    2,
    3,
    4,
    5
};

GradientDescentRegression.Compute(learningRate, epoch, x, y, true, 2);
Console.WriteLine("Press any key to quit the application");
Console.ReadLine();