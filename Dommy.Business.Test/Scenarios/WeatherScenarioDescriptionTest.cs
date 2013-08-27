using Dommy.Business.Scenarios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using System.Threading.Tasks;
using Dommy.Business.Tools;
using Dommy.Business.Syntax;

namespace Dommy.Business.Test.Scenarios
{
    [TestClass]
    public class WeatherScenarioDescriptionTest : BaseTest
    {
        [TestMethod]
        public void CreateTest()
        {
            var kernel = this.CreateKernel();

            Scenario.InitKernel(kernel);

            kernel.Bind<EedomusHelper>().ToSelf()
                .WithConstructorArgument("apiAddr", "127.0.0.1")
                .WithConstructorArgument("apiUser", "user")
                .WithConstructorArgument("apiSecret", "secret");

            var s = kernel.Get<WeatherScenarioDescription>();

            s.Create();
        }
    }
}
