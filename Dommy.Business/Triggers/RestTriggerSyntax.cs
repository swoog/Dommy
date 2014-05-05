//-----------------------------------------------------------------------
// <copyright file="RestTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Triggers
{
    using Dommy.Business.Syntax;
    using Ninject;

    public class RestTriggerSyntax : Dommy.Business.Triggers.IRestTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public RestTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax RestTrigger(string url)
        {
            var t = this.kernel.Get<RestTrigger>();
            t.Url = url;
            return this.scenario.Trigger(t);
        }

        public ITriggerScenarioSyntax RestTrigger(string url, object data)
        {
            var t = this.kernel.Get<RestTrigger>();
            t.Url = url;
            t.Data = data;
            return this.scenario.Trigger(t);
        }
    }
}
