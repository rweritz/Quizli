using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Quizli
{
    public sealed class QuizCmd : RootCommand
    {
        public QuizCmd()
        {
            Name = "quiz";
            AddAlias("q");
            
            Handler = CommandHandler.Create(Execute);
        }

        private static void Execute()
        {
            Console.WriteLine("quiz executed");
        }
    }
}