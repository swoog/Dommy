using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business
{
    public interface ISpeechLogger
    {
        void ErrorRecognition(Actor actor, string p);

        void Say(Actor actor, string p);

        bool IgnoreRecognition { get; }
    }
}
