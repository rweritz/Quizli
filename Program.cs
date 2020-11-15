using System.CommandLine;

namespace Quizli
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var quizCmd = new QuizCmd();
            var pixelPicsCmd = new PixelPicsCmd();
            quizCmd.AddCommand(pixelPicsCmd);

            quizCmd.InvokeAsync(args).Wait();
            pixelPicsCmd.InvokeAsync(args).Wait();
        }

    }
}