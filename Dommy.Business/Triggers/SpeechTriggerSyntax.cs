using Ninject;
using Dommy.Business.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Triggers
{
    public class SpeechTriggerSyntax : ISpeechTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public SpeechTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax SpeechTrigger(params string[] sentences)
        {
            var t = this.kernel.Get<SpeechTrigger>();
            t.WithPrefix = true;
            t.Speech = sentences;
            return this.scenario.Trigger(t);
        }

        public ITriggerScenarioSyntax SpeechTrigger(double confidence, params string[] sentences)
        {
            var t = this.kernel.Get<SpeechTrigger>();
            t.WithPrefix = true;
            t.Confidence = confidence;
            t.Speech = sentences;
            return this.scenario.Trigger(t);
        }

        public ITriggerScenarioSyntax NoPrefixSpeechTrigger(params string[] sentences)
        {
            var t = this.kernel.Get<SpeechTrigger>();
            t.WithPrefix = false;
            t.Speech = sentences;
            return this.scenario.Trigger(t);
        }
    }
}
