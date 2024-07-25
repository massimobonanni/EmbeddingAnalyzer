using Azure.AI.OpenAI;
using EmbeddingAnalyzer.Core.Entities;
using EmbeddingAnalyzer.Core.Implementations;
using EmbeddingAnalyzer.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace EmbeddingAnalyzer.Console.Commands
{
    internal class RAGCommand : Command
    {
        public RAGCommand() : base("rag", "Given one or more texts and a text file with several texts, return the top sentences order by distance for each text in input.")
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

            var textOption = new Option<IEnumerable<string>>(
                name: "--text",
                description: "The text used to seach in the data file.")
            {
                IsRequired = true,
            };
            textOption.AddAlias("-t");
            textOption.AllowMultipleArgumentsPerToken = true;
            this.AddOption(textOption);

            var fileOption = new Option<string>(
                name: "--file",
                description: "The full path of the line feed file (.txt) contains all the text to use as knowledge base.")
            {
                IsRequired = true,
            };
            fileOption.AddAlias("-f");
            this.AddOption(fileOption);

            var topOption = new Option<int>(
                name: "--top",
                description: "The number of result to retrieve. Default is 5.")
            {
                IsRequired = false,
            };
            topOption.SetDefaultValue(5);
            this.AddOption(topOption);

            this.SetHandler(CommandHandler,
                endpointOption, apiKeyOption, modelNameOption, textOption, fileOption, topOption);
        }

        private async Task CommandHandler(Uri endpoint, string apiKey, string modelName, IEnumerable<string> inputTexts, string filepath, int top)
        {
            var embeddingService = new AzureOpenAIEmbeddingService(endpoint, apiKey, modelName);

            System.Console.WriteLine($"Opening file '{filepath}'...");
            var textLines = await File.ReadAllLinesAsync(filepath);


            System.Console.WriteLine($"Get Embedding from input texts...");
            var inputEmbeddings = new List<TextEmbedding>();
            foreach (var text in inputTexts)
            {
                System.Console.Write($"\tEmbedding '{text}'...");
                var embedding = await embeddingService.GetEmbedding(text);
                inputEmbeddings.Add(embedding);
                System.Console.WriteLine($" Cost {embedding.Usage.TotalTokens} tokens");
            }

            System.Console.WriteLine();
            System.Console.WriteLine($"Get Embedding from input file...");
            var fileEmbeddings = new List<TextEmbedding>();
            foreach (var line in textLines)
            {
                System.Console.Write($"\tEmbedding '{line}'...");
                var embedding = await embeddingService.GetEmbedding(line);
                fileEmbeddings.Add(embedding);
                System.Console.WriteLine($" Cost {embedding.Usage.TotalTokens} tokens");
            }

            System.Console.WriteLine();
            System.Console.WriteLine($"Calculate distances...");
            var groundings = new List<EmbeddingGrounding>();
            foreach (var inputText in inputEmbeddings)
            {
                var grounding = new EmbeddingGrounding() { Text = inputText };
                System.Console.WriteLine();
                System.Console.WriteLine($"Calculating distances for '{inputText.Text}'...");
                foreach (var fileEmbedding in fileEmbeddings)
                {
                    System.Console.Write($"\tCalculating distances from '{fileEmbedding.Text}'...");
                    var distance = CosineSimilarityCalculator.CalculateDistance(inputText, fileEmbedding);
                    grounding.Items.Add(distance);
                    System.Console.WriteLine($" distance {distance.Distance}");
                }
                groundings.Add(grounding);
            }

            foreach (var grounding in groundings)
            {
                System.Console.WriteLine();
                System.Console.WriteLine($"Listing top {top} for '{grounding.Text.Text}'...");
                var query = grounding.Items.OrderBy(x => x.Distance).Take(top);

                foreach (var item in query)
                {
                    System.Console.WriteLine($"\tDistance from '{item.Text2.Text}' - {item.Distance}");
                }
            }
        }
    }
}
