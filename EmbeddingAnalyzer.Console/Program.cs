
using EmbeddingAnalyzer.Console.Commands;
using EmbeddingAnalyzer.Core.Implementations;
using EmbeddingAnalyzer.Core.Interfaces;
using System.CommandLine;
using System.Runtime.CompilerServices;

namespace EmbeddingAnalyzer.Console
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("Console for demo on Embedding Models");

            rootCommand.AddCommand(new CalculateDistanceCommand());
            rootCommand.AddCommand(new RAGCommand());

            return await rootCommand.InvokeAsync(args);
        }
    }
}
