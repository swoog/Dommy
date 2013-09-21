//-----------------------------------------------------------------------
// <copyright file="ISayActions.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    using System;
    using Dommy.Business.Syntax;

    /// <summary>
    /// Eedomus syntax.
    /// </summary>
    public interface IEedomusActions : IExtendSyntax
    {
        IScenarioSyntax EedomusCall(EedomusApi api, EedoumusAction action, string eedomusId, string value = null);

        IScenarioSyntax EedomusCall(EedoumusAction action, string eedomusId, string value = null);
    
        IScenarioSyntax EedomusOnOff(string id, bool isOn);

        IScenarioSyntax EedomusValue(string id, double value);
    }
}
