using Azure.AI.OpenAI;
using EmbeddingAnalyzer.Core.Implementations;
using EmbeddingAnalyzer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddingAnalyzer.Console.Commands
{
    internal class CalculateDistanceCommand : Command
    {
        public CalculateDistanceCommand() : base("calculate-distance", "Calculate the distance between two texts.")
        {
            var endpointOption = new Option<Uri>(
                name: "--endpoint",
                description: "The endpoint of Azure OpenAI resource.")
            {
                IsRequired = true,
            };
            endpointOption.AddAlias("-e");
            this.AddOption(endpointOption);

            var apiKeyOption = new Option<string>(
                name: "--api-key",
                description: "The API key of Azure OpenAI resource.")
            {
                IsRequired = true,
            };
            apiKeyOption.AddAlias("-k");
            this.AddOption(apiKeyOption);

            var modelNameOption = new Option<string>(
                name: "--model-name",
                description: "The model name of Azure OpenAI resource.")
            {
                IsRequired = true,
            };
            modelNameOption.AddAlias("-m");
            this.AddOption(modelNameOption);

            var text1Option = new Option<string>(
                name: "--text1",
                description: "The first text.")
            {
                IsRequired = true,
            };
            text1Option.AddAlias("-t1");
            this.AddOption(text1Option);

            var text2Option = new Option<string>(
                name: "--text2",
                description: "The second text.")
            {
                IsRequired = true,
            };
            text2Option.AddAlias("-t2");
            this.AddOption(text2Option);

            this.SetHandler(CommandHandler,
                endpointOption,apiKeyOption,modelNameOption,text1Option,text2Option);
        }

        private async Task CommandHandler(Uri endpoint, string apiKey, string modelName, string text1, string text2)
        {
            var embeddingService = new AzureOpenAIEmbeddingService(endpoint,apiKey,modelName);

            System.Console.WriteLine($"Calculating distance between '{text1}' and '{text2}'...");
            System.Console.WriteLine();

            System.Console.Write($"Embedding '{text1}'...");
            var embedding1 = await embeddingService.GetEmbedding(text1);
            System.Console.WriteLine($" Cost {embedding1.Usage.TotalTokens} tokens");

            System.Console.Write($"Embedding '{text2}'...");
            var embedding2 = await embeddingService.GetEmbedding(text2);
            System.Console.WriteLine($" Cost {embedding2.Usage.TotalTokens} tokens");
            
            System.Console.WriteLine();
            
            System.Console.WriteLine("Calculating distance...");
            var distance = CosineSimilarityCalculator.CalculateDistance(embedding1, embedding2);

            System.Console.WriteLine($"Distance {distance}");

            System.Console.WriteLine();
        }
    }
}
