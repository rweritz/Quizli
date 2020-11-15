using System.CommandLine;

namespace Quizli
{
    class Program
    {
        static void Main(string[] args)
        {
            var rootCommand = new RootCommand("quiz");
            rootCommand.AddAlias("q");
            var pixelPicsCmd = new PixelPicsCmd();
            rootCommand.AddCommand(pixelPicsCmd);

            rootCommand.InvokeAsync(args).Wait();
        }

    }
}