using SGDOptimization;
using System.Diagnostics;

// https://jamesmccaffrey.wordpress.com/2011/10/09/quickly-selecting-a-random-line-from-a-huge-text-file-using-seek/

void ExecuteSGD(IMathUtils mathUtils, string fileName, bool isParallel)
{
    int epoch = 10;
    double learningRate = 0.05;
    var y = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Files", "y.csv"))
        .Skip(1)
        .Select(d => double.Parse(d.Split(';').Last().Replace('.', ',')))
        .ToArray();
    var xVector = System.IO.File.ReadAllLines(Path.Combine(Directory.GetCurrentDirectory(), "Files", "x.csv"))
        .Skip(1)
        .Select(d => d.Split(';').Skip(1).Select(n => double.Parse(n.Replace('.', ','))))
        .ToArray();
    var x = new double[y.Length, xVector.First().Count()];
    Buffer.BlockCopy(xVector.SelectMany(r => r).ToArray(),
                        0,
                        x,
                        0,
                        y.Length * xVector.First().Count());

    // var regression = new GradientDescentRegression(GPUMathUtils.New());
    var regression = new GradientDescentRegression(mathUtils);

    var sw = new Stopwatch();
    sw.Start();
    var statResult = regression.Compute(learningRate, epoch, x, y, 500, 2, isParallel);
    sw.Stop();

    Write(statResult, fileName);

    Console.WriteLine($"Executed in '{sw.ElapsedMilliseconds}' MS");
    Console.WriteLine("Press any key to quit the application");
    Console.ReadLine();
}

void Write(SgdStatResult stat, string fileName)
{
    var output = Path.Combine(Directory.GetCurrentDirectory(), fileName);
    if (File.Exists(output))
        File.Delete(output);

    var lines = new List<string>
    {
        "Epoch;ExecutionTime"
    };
    foreach (var elt in stat.ExecutionTimes)
        lines.Add($"{elt.Key};{elt.Value}");

    File.WriteAllLines(output, lines);
}

// ExecuteSGD(new CPUMathUtils(), "seq-cpu.csv", false);
// ExecuteSGD(GPUMathUtils.New(), "seq-gpu.csv", false);
// 
// ExecuteSGD(new CPUMathUtils(), "parallel-cpu.csv", true);
ExecuteSGD(GPUMathUtils.New(), "parallel-gpu.csv", true);