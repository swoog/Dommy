//-----------------------------------------------------------------------
// <copyright file="ITrigger.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Triggers
{
    using Dommy.Business.Scenarios;

    public interface ITrigger
    {
        void Init(Engine currentEngine, IScenario currentScenario);
    }
}
