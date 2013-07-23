using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business
{
    public class Sentence : ISentence
    {
        public string[] Words { get; set; }

        public string Text { get; set; }

        public float Confidence { get; set; }

        public float[] WordsConfidence { get; set; }
    }
}
