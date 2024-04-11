// See https://aka.ms/new-console-template for more information

// implémenter cross correlation.
// implémenter convolution.

using CNN;
using static CNN.Picture;

var imageExtractionResult = Picture.Extract(Path.Combine(Directory.GetCurrentDirectory(), "other-picture.png"));
var filter = new int[,]
{
    { 1, 2, 1 },
    { 0, 0, 0 },
    { -1, -2, -1 }
};

var blueM = ConvolutionPicture.Transform(imageExtractionResult.BlueMatrix, filter);
var redM = ConvolutionPicture.Transform(imageExtractionResult.RedMatrix, filter);
var greeM = ConvolutionPicture.Transform(imageExtractionResult.GreenMatrix, filter);
Picture.Save(Path.Combine(Directory.GetCurrentDirectory(), "other-picture-blue.png"), new ImageExtractionResult
{
    BlueMatrix = blueM,
    RedMatrix = redM,
    GreenMatrix = greeM
});
// save RGB to file.

Console.WriteLine("Hello, World!");