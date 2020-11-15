using System.IO;
using System.Linq;
using PdfSharpCore.Drawing;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Advanced;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace Quizli
{
    public static class ImageExtensions
    {
        public static MemoryStream ToMemoryStream(this Image image)
        {
            var memoryStream = new MemoryStream();
            image.Save(memoryStream,  new JpegEncoder());
            memoryStream.Position = 0;
            return memoryStream;
        }
        
        public static XImage ToXImage(this Image image)
        {
            var xImage = XImage.FromStream(image.ToMemoryStream);
            return xImage;
        }
    }
}