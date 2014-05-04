
namespace Dommy.Business.Triggers
{
    using System;
    using Dommy.Business.Syntax;

    public interface ISpeechTriggerSyntax : IExtendSyntax
    {
        ITriggerScenarioSyntax NoPrefixSpeechTrigger(params string[] sentences);
        ITriggerScenarioSyntax SpeechTrigger(params string[] sentences);
        ITriggerScenarioSyntax SpeechTrigger(double confidence, params string[] sentences);
    }
}
