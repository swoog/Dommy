//-----------------------------------------------------------------------
// <copyright file="StartupTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Triggers
{
    using Dommy.Business.Syntax;
    using Ninject;

    public class StartupTriggerSyntax : IStartupTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public StartupTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax StartupTrigger()
        {
            var t = this.kernel.Get<StartupTrigger>();
            return this.scenario.Trigger(t);
        }
    }
}
