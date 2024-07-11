using EmbeddingAnalyzer.Core.Entities;

public class CosineSimilarityCalculator
{
    public static EmbeddingDistance CalculateDistance(TextEmbedding embedding1, TextEmbedding embedding2)
    {
        return new EmbeddingDistance()
        {
            Text1 = embedding1,
            Text2 = embedding2,
            Distance = 1.0f - Calculate(embedding1.Embedding, embedding2.Embedding)
        };
    }

    public static float Calculate(IEnumerable<float> vector1, IEnumerable<float> vector2)
    {
        if (vector1 == null || vector2 == null)
        {
            throw new ArgumentNullException("Vectors must not be null.");
        }

        if (vector1.Count() != vector2.Count())
        {
            throw new ArgumentException("Vectors must be of the same length.");
        }

        float dotProduct = 0.0f;
        float normA = 0.0f;
        float normB = 0.0f;
        for (int i = 0; i < vector1.Count(); i++)
        {
            dotProduct += vector1.ElementAt(i) * vector2.ElementAt(i);
            normA += (float)Math.Pow((double)vector1.ElementAt(i), 2);
            normB += (float)Math.Pow((double)vector2.ElementAt(i), 2);
        }
        return (float) (dotProduct / (Math.Sqrt((double)normA) * (double)Math.Sqrt(normB)));
    }
}
