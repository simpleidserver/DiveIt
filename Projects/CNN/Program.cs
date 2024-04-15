// See https://aka.ms/new-console-template for more information

// implémenter cross correlation.
// implémenter convolution.

using CNN;
using static CNN.Picture;

void ReadData()
{
    var content = File.ReadAllLines("c:\\Projects\\DiveIt\\Projects\\CNN\\A_Z Handwritten Data.csv")
        .Select(s => s.Split(',').Select(n => int.Parse(n)));
    var trainSize = 0.2;
    var numberToTrain = (int)(content.Count() * trainSize);
    var trainData = content.Take(numberToTrain);
    var trainX = trainData.Select(s => MatrixHelper.Reshape(s.Skip(1).ToArray(), 28, 28));
    var trainY = trainData.Select(s => s.First()).ToList();

}

void Convulation()
{
    // https://en.wikipedia.org/wiki/Kernel_(image_processing)
    // edge detection.
    // var filter = new int[,]
    // {
    //     { -1, -1, -1 },
    //     { -1, 8, -1 },
    //     { -1, -1, -1 }
    // };
    var filter = new int[,]
    {
        { 0, 1 },
        { 2, 3 }
    };
    // var grayScale = Picture.ExtractGrayScale(Path.Combine(Directory.GetCurrentDirectory(), "picture.png"));
    var grayScale = new int[,]
    {
        { 0, 1, 2 },
        { 3, 4, 5 },
        { 6, 7, 8 }
    };
    var newGrayScale = ConvolutionAlg.Transform(grayScale, filter, filter.GetLength(0) - 1);
    Picture.Save(Path.Combine(Directory.GetCurrentDirectory(), "other-picture-transformed.png"), new ImageExtractionResult
    {
        BlueMatrix = newGrayScale,
        RedMatrix = newGrayScale,
        GreenMatrix = newGrayScale
    });
}

void MaxPool()
{
    var matrix = new int[,]
    {
        { 0, 50, 0, 29 },
        { 0, 80, 31, 2 },
        { 33, 90, 0, 75 },
        { 0, 9, 0, 95 }
    };
    var r = Pooling.MaxPool(matrix, 2, 2);
    string ss = "";
}

// ExtractGrayScale();
// Convulation();
// MaxPool();
ReadData();

Console.WriteLine("Hello, World!");