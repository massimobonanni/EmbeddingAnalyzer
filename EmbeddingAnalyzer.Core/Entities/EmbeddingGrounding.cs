using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddingAnalyzer.Core.Entities
{
    public class EmbeddingGrounding
    {
        public EmbeddingGrounding()
        {
            this.Items = new List<EmbeddingDistance>();
        }

        public TextEmbedding Text { get; set; }

        public ICollection<EmbeddingDistance> Items { get; set; }
    }
}
