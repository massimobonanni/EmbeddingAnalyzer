using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddingAnalyzer.Core.Entities
{
    public class EmbeddingDistance
    {
        public TextEmbedding Text1 { get; set; }
        public TextEmbedding Text2 { get; set; }

        public float Distance { get; set; }
    }
}
