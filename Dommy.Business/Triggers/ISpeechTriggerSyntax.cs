using Dommy.Business.Syntax;
using System;

namespace Dommy.Business.Triggers
{
    public interface ISpeechTriggerSyntax : IExtendSyntax
    {
        ITriggerScenarioSyntax NoPrefixSpeechTrigger(params string[] sentences);
        ITriggerScenarioSyntax SpeechTrigger(params string[] sentences);
        ITriggerScenarioSyntax SpeechTrigger(double confidence, params string[] sentences);
    }
}
