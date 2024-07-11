using EmbeddingAnalyzer.Core.Entities;
using System.ClientModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EmbeddingAnalyzer.Core.Interfaces;
using Azure;
using Azure.AI.OpenAI;

namespace EmbeddingAnalyzer.Core.Implementations
{
    public class AzureOpenAIEmbeddingService : IEmbeddingService
    {
        private readonly Uri Endpoint;
        private readonly string ApiKey;
        private readonly string ModelName;
        private readonly OpenAIClient openAIClient;

        public AzureOpenAIEmbeddingService(Uri endpoint, string apiKey, string modelName)
        {
            this.Endpoint = endpoint;
            this.ApiKey = apiKey;
            this.ModelName = modelName;

            AzureKeyCredential credentials = new(ApiKey);
            openAIClient = new(this.Endpoint, credentials);
        }

        public async Task<TextEmbedding> GetEmbedding(string text)
        {
            TextEmbedding returnValue = null;

            EmbeddingsOptions embeddingOptions = new()
            {
                DeploymentName = this.ModelName,
                Input = { text },
            };

            try
            {
                var response = await openAIClient.GetEmbeddingsAsync(embeddingOptions);

                returnValue = new TextEmbedding()
                {
                    Text = text,
                    Embedding = response.Value.Data[0].Embedding.ToArray(),
                    Usage = new ServiceUsage()
                    {
                        PromptTokens = response.Value.Usage.PromptTokens,
                        CompletionTokens = response.Value.Usage.TotalTokens - response.Value.Usage.PromptTokens,
                        TotalTokens = response.Value.Usage.TotalTokens,
                    },
                };
            }
            catch (Exception ex)
            {
                returnValue = null;
            }

            return returnValue;
        }

    }
}
