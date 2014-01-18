//-----------------------------------------------------------------------
// <copyright file="IScenario.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Dommy.Business.Triggers;

    /// <summary>
    /// Interface of a scenario.
    /// </summary>
    public interface IScenario
    {
        /// <summary>
        /// Gets or sets trigger of this scenario.
        /// </summary>
        IList<ITrigger> Triggers { get; set; }

        /// <summary>
        /// Gets scenario name.
        /// </summary>
        string ScenarioName { get; }

        /// <summary>
        /// Initialize scenario with the instance of engine.
        /// </summary>
        /// <param name="engine">Core engine to use with this scenario.</param>
        void Init(Engine engine);

        /// <summary>
        /// Run scenario.
        /// </summary>
        void Run();

        /// <summary>
        /// Start scenario asynchronously.
        /// </summary>
        /// <returns>Return Task to wait on this.</returns>
        Task RunAsync();
    }
}
