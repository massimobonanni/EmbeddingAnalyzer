using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmbeddingAnalyzer.Core.Entities
{
    public class TextEmbedding
    {
        public string Text { get; set; }

        public IEnumerable<float> Embedding { get; set; }

        public ServiceUsage Usage { get; set; }
    }
}
