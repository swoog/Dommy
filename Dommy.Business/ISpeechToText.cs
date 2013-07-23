using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business
{
    public interface ISpeechToText
    {
        void Init();

        void Start(Action<ISentence> action);

        void LoadGrammar(Microsoft.Speech.Recognition.Grammar grammar);

        void Stop();

        void UnloadGrammar(Microsoft.Speech.Recognition.Grammar grammar);
    }
}
