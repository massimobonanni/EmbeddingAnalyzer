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
            var endpointOption = new Option<Uri>("--endpoint") { Description = "The endpoint of Azure OpenAI resource.", Required = true };
            endpointOption.Aliases.Add("-e");
            this.Options.Add(endpointOption);

            var apiKeyOption = new Option<string>("--api-key") { Description = "The API key of Azure OpenAI resource.", Required = true };
            apiKeyOption.Aliases.Add("-k");
            this.Options.Add(apiKeyOption);

            var modelNameOption = new Option<string>("--model-name") { Description = "The model name of Azure OpenAI resource.", Required = true };
            modelNameOption.Aliases.Add("-m");
            this.Options.Add(modelNameOption);

            var text1Option = new Option<string>("--text1") { Description = "The first text.", Required = true };
            text1Option.Aliases.Add("-t1");
            this.Options.Add(text1Option);

            var text2Option = new Option<string>("--text2") { Description = "The second text.", Required = true };
            text2Option.Aliases.Add("-t2");
            this.Options.Add(text2Option);

            this.SetAction(async context =>
            {
                var endpoint = context.GetValue(endpointOption)!;
                var apiKey = context.GetValue(apiKeyOption)!;
                var modelName = context.GetValue(modelNameOption)!;
                var text1 = context.GetValue(text1Option)!;
                var text2 = context.GetValue(text2Option)!;

                var embeddingService = new AzureOpenAIEmbeddingService(endpoint, apiKey, modelName);

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
            });
        }
    }
}
