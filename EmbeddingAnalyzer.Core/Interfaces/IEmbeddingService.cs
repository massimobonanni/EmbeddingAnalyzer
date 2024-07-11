using EmbeddingAnalyzer.Core.Entities;

namespace EmbeddingAnalyzer.Core.Interfaces
{
    public interface IEmbeddingService
    {
        Task<TextEmbedding> GetEmbedding(string text);
    }
}