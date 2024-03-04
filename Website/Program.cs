// See https://aka.ms/new-console-template for more information


using SGDOptimization;

void Test()
{
    int epoch = 1000;
    double learningRate = 0.05;
    var y = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Files", "y.csv"))
        .Split(';')
        .Select(d => double.Parse(d.Replace('.', ',')))
        .ToArray();
    var x = new double[y.Length, 1];
    var xVector = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Files", "x.csv"))
        .Split(';')
        .Select(d => double.Parse(d.Replace('.', ',')))
        .ToArray();
    for (var i = 0; i < xVector.Count(); i++)
        x[i, 0] = xVector[i];

    GradientDescentRegression.Compute(learningRate, epoch, x, y, 500, 5);
    Console.WriteLine("Press any key to quit the application");
    Console.ReadLine();
}

var x = new double[,]
{
    { 1, 1 }
};
var y = new double[]
{
    2,
    2
};
GPUMathUtils.Init();
GPUMathUtils.Multiply(x, y);