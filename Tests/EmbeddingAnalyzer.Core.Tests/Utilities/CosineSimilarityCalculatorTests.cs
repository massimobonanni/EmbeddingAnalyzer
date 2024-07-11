using EmbeddingAnalyzer.Core.Entities;
using System;
using Xunit;

namespace EmbeddingAnalyzer.Core.Tests.Utilities
{
    public class CosineSimilarityCalculatorTests
    {
        #region [ Calculate ]
        [Theory]
        [InlineData(new float[] { 0.5774f, 0.5774f, 0.5774f }, new float[] { 0.5774f, 0.5774f, 0.5774f }, 1)] // Both vectors are the same.
        [InlineData(new float[] { 0.7071f, 0.7071f }, new float[] { -0.7071f, -0.7071f }, -1)] // Opposite vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f }, new float[] { 0.7071f, -0.7071f }, 0)] // Orthogonal vectors.
        [InlineData(new float[] { 0.5774f, 0.5774f, 0.5774f, 0.5774f }, new float[] { 0.5774f, 0.5774f, 0.5774f, 0.5774f }, 1)] // 4-dimensional vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f, 0.7071f, 0.7071f }, new float[] { -0.7071f, -0.7071f, -0.7071f, -0.7071f }, -1)] // 4-dimensional vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f, 0.7071f, 0.7071f }, new float[] { 0.7071f, -0.7071f, 0.7071f, -0.7071f }, 0)] // 4-dimensional vectors.
        [InlineData(new float[] { 0.5774f, 0.5774f, 0.5774f, 0.5774f, 0.5774f }, new float[] { 0.5774f, 0.5774f, 0.5774f, 0.5774f, 0.5774f }, 1)] // 5-dimensional vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f, 0.7071f, 0.7071f, 0.7071f }, new float[] { -0.7071f, -0.7071f, -0.7071f, -0.7071f, -0.7071f }, -1)] // 5-dimensional vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f, 0.7071f, 0.7071f, 0.7071f }, new float[] { 0.7071f, -0.7071f, 0.7071f, -0.7071f, 0.7071f }, 0.2f)] // 5-dimensional vectors.
        public void Calculate_WithvalidVectors_ReturnsExpectedResult(float[] vector1, float[] vector2, double expected)
        {
            double result = CosineSimilarityCalculator.Calculate(vector1, vector2);
            Assert.Equal(expected, result, 4);
        }

        [Theory]
        [InlineData(new float[] { 0.7071f, 0.7071f }, new float[] { 0.7071f })]
        [InlineData(null, new float[] { 0.7071f, 0.7071f })]
        [InlineData(new float[] { 0.7071f, 0.7071f }, null)]
        public void Calculate_WithInvalidVectors_ThrowsException(float[] vector1, float[] vector2)
        {
            Assert.ThrowsAny<ArgumentException>(() => CosineSimilarityCalculator.Calculate(vector1, vector2));
        }

        [Theory]
        [InlineData(1000)]
        [InlineData(5000)]
        public void Calculate_WithLargeNormalizedVectors_CalculatesCorrectly(int size)
        {
            float[] vector1 = new float[size];
            float[] vector2 = new float[size];
            for (int i = 0; i < size; i++)
            {
                vector1[i] = 1f / (float)Math.Sqrt(size);
                vector2[i] = 1f / (float)Math.Sqrt(size);
            }
            double result = CosineSimilarityCalculator.Calculate(vector1, vector2);
            Assert.Equal(1, result,4);
        }
        #endregion [ Calculate ]

        #region [ CalculateDistance ]
        [Theory]
        [InlineData(new float[] { 0.5774f, 0.5774f, 0.5774f }, new float[] { 0.5774f, 0.5774f, 0.5774f }, 0)] // Both vectors are the same.
        [InlineData(new float[] { 0.7071f, 0.7071f }, new float[] { -0.7071f, -0.7071f }, 2)] // Opposite vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f }, new float[] { 0.7071f, -0.7071f }, 1)] // Orthogonal vectors.
        [InlineData(new float[] { 0.5774f, 0.5774f, 0.5774f, 0.5774f }, new float[] { 0.5774f, 0.5774f, 0.5774f, 0.5774f }, 0)] // 4-dimensional vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f, 0.7071f, 0.7071f }, new float[] { -0.7071f, -0.7071f, -0.7071f, -0.7071f }, 2)] // 4-dimensional vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f, 0.7071f, 0.7071f }, new float[] { 0.7071f, -0.7071f, 0.7071f, -0.7071f }, 1)] // 4-dimensional vectors.
        [InlineData(new float[] { 0.5774f, 0.5774f, 0.5774f, 0.5774f, 0.5774f }, new float[] { 0.5774f, 0.5774f, 0.5774f, 0.5774f, 0.5774f }, 0)] // 5-dimensional vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f, 0.7071f, 0.7071f, 0.7071f }, new float[] { -0.7071f, -0.7071f, -0.7071f, -0.7071f, -0.7071f }, 2)] // 5-dimensional vectors.
        [InlineData(new float[] { 0.7071f, 0.7071f, 0.7071f, 0.7071f, 0.7071f }, new float[] { 0.7071f, -0.7071f, 0.7071f, -0.7071f, 0.7071f }, 0.8f)] // 5-dimensional vectors.
        public void CalculateDistance_WithValidEmbeddings_ReturnsExpectedResult(float[] vector1, float[] vector2, double expected)
        {
            var embedding1 = new TextEmbedding()
            {
                Text = "Text 1",
                Embedding = vector1,
            };
            var embedding2 = new TextEmbedding()
            {
                Text = "Text 2",
                Embedding = vector2,
            };

            var result = CosineSimilarityCalculator.CalculateDistance(embedding1, embedding2);
            Assert.Equal(expected, result.Distance, 4);
        }
        #endregion [ CalculateDistance ]
    }
}