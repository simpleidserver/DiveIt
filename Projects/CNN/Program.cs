// See https://aka.ms/new-console-template for more information

// implémenter cross correlation.
// implémenter convolution.

using CNN;
using static CNN.Picture;
void Convulation()
{
    // https://en.wikipedia.org/wiki/Kernel_(image_processing)
    // edge detection.
    var filter = new int[,]
    {
        { -1, -1, -1 },
        { -1, 8, -1 },
        { -1, -1, -1 }
    };
    var grayScale = Picture.ExtractGrayScale(Path.Combine(Directory.GetCurrentDirectory(), "picture.png"));
    /*
    var grayScale = new int[,]
    {
        { 0, 1, 2 },
        { 3, 4, 5 },
        { 6, 7, 8 }
    };
    */
    var newGrayScale = ConvolutionPicture.Transform(grayScale, filter, filter.GetLength(0) - 1);
    Picture.Save(Path.Combine(Directory.GetCurrentDirectory(), "other-picture-transformed.png"), new ImageExtractionResult
    {
        BlueMatrix = newGrayScale,
        RedMatrix = newGrayScale,
        GreenMatrix = newGrayScale
    });
}

// ExtractGrayScale();
Convulation();
// save RGB to file.

Console.WriteLine("Hello, World!");