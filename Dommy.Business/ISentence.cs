using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business
{
    public interface ISentence
    {
        string[] Words { get; }

        float[] WordsConfidence { get; }

        string Text { get; }

        float Confidence { get; }
    }
}
