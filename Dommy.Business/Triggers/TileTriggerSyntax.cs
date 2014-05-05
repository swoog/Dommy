﻿//-----------------------------------------------------------------------
// <copyright file="TileTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Triggers
{
    using System.Drawing;
    using Dommy.Business.Syntax;
    using Ninject;

    public class TileTriggerSyntax : ITileTriggerSyntax
    {
        private IKernel kernel;
        private ITriggerSyntax scenario;

        public TileTriggerSyntax(IKernel kernel, ITriggerSyntax scenario)
        {
            this.kernel = kernel;
            this.scenario = scenario;
        }

        public ITriggerScenarioSyntax TileTrigger(string title, Color backGround)
        {
            var t = this.kernel.Get<TileTrigger>();
            t.Title = title;
            t.BackgroundColor = backGround;
            return this.scenario.Trigger(t);
        }
    }
}
