using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business
{
    public class Sentence : ISentence
    {
        public ICollection<string> Words { get; set; }

        public string Text { get; set; }

        public float Confidence { get; set; }

        public ICollection<float> WordsConfidence { get; set; }
    }
}
