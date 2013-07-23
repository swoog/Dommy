using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Result
{
    public class PrecisionResult : SayResult
    {
        public IList<SentenceAction> SentenceActions { get; private set; }

        public PrecisionResult(string speech, IList<SentenceAction> sentenceActions)
            :base(speech)
        {
            this.SentenceActions = sentenceActions;
        }

        public class SentenceAction
        {
            public string UniqueKey { get; set; }

            public IList<string> Sentences { get; set; }

            public Func<ISentence, IResult> Action { get; set; }
        }
    }
}
