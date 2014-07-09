//-----------------------------------------------------------------------
// <copyright file="Scenario.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Syntax
{
    using System;
    using Dommy.Business.Actions;
    using Dommy.Business.Scenarios;
    using Ninject;
    using Ninject.Parameters;

    /// <summary>
    /// Static class used to create scenarios.
    /// </summary>
    public static class Scenario
    {
        /// <summary>
        /// Ninject kernel.
        /// </summary>
        private static IKernel kernel;

        /// <summary>
        /// Initialize Ninject kernel.
        /// </summary>
        /// <param name="kernel">Ninject kernel.</param>
        public static void InitKernel(IKernel kernel)
        {
            Scenario.kernel = kernel;
        }

        /// <summary>
        /// Create a new scenario.
        /// </summary>
        /// <param name="name">Name of the scenario.</param>
        /// <returns>Object used to define a scenario.</returns>
        public static ITriggerSyntax Create(string name = null)
        {
            return Scenario.kernel.Get<ScenarioSyntax>(new ConstructorArgument("name", name));
        }

        /// <summary>
        /// Create a scenario from scenario description.
        /// </summary>
        /// <typeparam name="T">Scenario description.</typeparam>
        /// <returns>Object used to define configuration of the scenario description.</returns>
        public static ScenarioDescriptionConfig<T> CreateFrom<T>()
            where T : IScenarioDescription
        {
            var c = Scenario.kernel.Bind<IScenarioDescription>().To<T>().InSingletonScope();

            return new ScenarioDescriptionConfig<T>(c);
        }

        /// <summary>
        /// Build all scenario definitions.
        /// </summary>
        public static void Build()
        {
            var descriptions = Scenario.kernel.GetAll<IScenarioDescription>();

            Exception ex = null;

            foreach (var d in descriptions)
            {
                try
                {
                    d.Create();
                }
                catch (Exception ex2)
                {
                    ex = ex2;
                }
            }

            if (ex != null)
            {
                throw ex;
            }
        }
    }
}
