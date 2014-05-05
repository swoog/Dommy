//-----------------------------------------------------------------------
// <copyright file="StartupTrigger.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Triggers
{
    public class StartupTrigger : ITrigger
    {
        public void Init(Engine currentEngine, Scenarios.IScenario currentScenario)
        {
            currentScenario.Run();
        }
    }
}
