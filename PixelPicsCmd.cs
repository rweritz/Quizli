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
            Handler = CommandHandler.Create(Execute);
        }
        
        private static void Execute()
        {
            var folder = @"E:\OneDrive\Quiz\Quiz3\Bilder";
            //var fileName = @"colosseum";
            var fileType = "jpg";

            var imageFilenames = Directory.EnumerateFiles(folder).Where(f => f.EndsWith(fileType)).ToList();
            var xImages = CreateXImages(imageFilenames, 4, 3, 50);
            
            GeneratePdf(xImages, Path.Combine(folder, $"result.pdf"));
        }
        
        private static IList<XImage> CreateXImages(IList<string> imageFilenames, int wInc, int hInc, int imageCount)
        {
            var result = new List<XImage>();
            
            foreach (var imageFilename in imageFilenames)
            {
                using var image = Image.Load(imageFilename, new JpegDecoder());
                
                int width;
                int height;

                for (var i = 1; i < imageCount; i += 2)
                {
                    width = wInc * i;
                    height = hInc * i;
                    var imageClone = image.Clone(i => i.Resize(width, height));
                    var xImage = imageClone.ToXImage();
                
                    result.Add(xImage);
                }

                width = 1600;
                height = 1200;
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