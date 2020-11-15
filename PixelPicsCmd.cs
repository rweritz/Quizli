using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Quizli
{
    public class PixelPicsCmd : Command
    {
        public PixelPicsCmd() : base("pixelpics")
        {
            var folderOption = new Option<string>("--folder", () => Environment.CurrentDirectory, "folder path including the pictures");
            folderOption.AddAlias("-f");
            folderOption.IsRequired = false;
            AddOption(folderOption);
            
            var widthOption = new Option<int>("--width", () => 1600, "the width in pixel (px) of the best quality picture");
            widthOption.AddAlias("-w");
            folderOption.IsRequired = false;
            AddOption(widthOption);
            
            var heightOption = new Option<int>("--height",() => 1200, "the heigth in pixel (px) of the best quality picture");
            heightOption.AddAlias("-h");
            folderOption.IsRequired = false;
            AddOption(heightOption);
            
            var wIncOption = new Option<int>("--width-increment", () => 4, "the pixel count to increment the width per picture");
            wIncOption.AddAlias("-wi");
            folderOption.IsRequired = false;
            AddOption(wIncOption);
            
            var hIncOption = new Option<int>("--height-increment",() => 3, "the pixel count to increment the height per picture");
            hIncOption.AddAlias("-hi");
            folderOption.IsRequired = false;
            AddOption(hIncOption);
            
            var imageCountOption = new Option<int>("--image-count", () => 25, "count of images used in the result pdf");
            imageCountOption.AddAlias("-c");
            folderOption.IsRequired = false;
            AddOption(imageCountOption);

            Handler = CommandHandler.Create<string, int, int, int, int, int>(Execute);
        }
        
        private static void Execute(string folder, int width, int height, int wInc, int hInc, int imageCount)
        {
            var fileType = "jpg";

            var imageFilenames = Directory.EnumerateFiles(folder).Where(f => f.EndsWith(fileType)).ToList();
            var xImages = CreateXImages(imageFilenames, width, height, wInc, hInc, imageCount);
            //var xImages = CreateXImages(imageFilenames, 4, 3, 50, 1200, 1600);
            
            GeneratePdf(xImages, Path.Combine(folder, $"result.pdf"));
        }
        
        private static IList<XImage> CreateXImages(IList<string> imageFilenames, int width, int height, int wInc, int hInc, int imageCount)
        {
            var result = new List<XImage>();
            
            foreach (var imageFilename in imageFilenames)
            {
                using var image = Image.Load(imageFilename, new JpegDecoder());

                var iterationCount = imageCount * 2 - 2;
                for (var i = 1; i < iterationCount; i += 2)
                {
                    var w = wInc * i;
                    var h = hInc * i;
                    var imageClone = image.Clone(i => i.Resize(w, h));
                    var xImage = imageClone.ToXImage();
                
                    result.Add(xImage);
                }
                
                var finalImage = image.Clone(i => i.Resize(width, height));
                var finalXImage = finalImage.ToXImage();
                result.Add(finalXImage);
            }
            
            return result;
        }

        
        private static void GeneratePdf(IList<XImage> xImages, string pdfPath)
        {
            var document = new PdfDocument();

            foreach (var xImage in xImages)
            {
                var page = document.AddPage();
                page.Width = 1600;
                page.Height = 1200;

                var gfx = XGraphics.FromPdfPage(page);
                gfx.DrawImage(xImage, 0, 0, 1600, 1200);
            }

            document.Save(pdfPath);
        }
        
    }
}