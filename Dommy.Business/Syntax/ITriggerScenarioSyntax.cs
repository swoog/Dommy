//-----------------------------------------------------------------------
// <copyright file="ITriggerScenarioSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Syntax
{
    using Ninject;

    /// <summary>
    /// Syntax indicating the trigger and actions.
    /// </summary>
    public interface ITriggerScenarioSyntax : ITriggerSyntax, IScenarioSyntax
    {
    }
}
