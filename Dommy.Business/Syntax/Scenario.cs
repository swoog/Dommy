
namespace Dommy.Business.Syntax
{
    using System;
    using Dommy.Business.Actions;
    using Dommy.Business.Scenarios;
    using Ninject;
    using Ninject.Parameters;

    public static class Scenario
    {
        private static IKernel kernel;

        public static void InitKernel(IKernel kernel)
        {
            Scenario.kernel = kernel;
        }

        public static ITriggerSyntax Create(string name = null)
        {
            return Scenario.kernel.Get<ScenarioSyntax>(new ConstructorArgument("name", name));
        }

        public static ScenarioDescriptionConfig<T> CreateFrom<T>()
            where T : IScenarioDescription
        {
            var c = Scenario.kernel.Bind<IScenarioDescription>().To<T>().InSingletonScope();

            return new ScenarioDescriptionConfig<T>(c);
        }

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
