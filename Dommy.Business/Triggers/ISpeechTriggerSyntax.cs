using Dommy.Business.Syntax;
using System;

namespace Dommy.Business.Triggers
{
    public interface ISpeechTriggerSyntax : IExtendSyntax
    {
        IScenarioSyntax NoPrefixSpeechTrigger(params string[] sentences);
        IScenarioSyntax SpeechTrigger(params string[] sentences);
        IScenarioSyntax SpeechTrigger(double confidence, params string[] sentences);
    }
}
