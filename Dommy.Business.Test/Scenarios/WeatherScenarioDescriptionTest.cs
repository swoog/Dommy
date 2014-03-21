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
        public void CreateAndRunTest()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<IWebRequest>().ToConstant(new MockWebRequest(new Dictionary<string, string>
            {
                {"http://127.0.0.1/api/get?action=periph.caract&periph_id=&value=&api_user=user&api_secret=secret", "scenarios/HttpEedomusTemperature.txt"}
            }));

            Scenario.InitKernel(kernel);

            kernel.Bind<EedomusHelper>().ToSelf()
                .WithConstructorArgument("apiAddr", "127.0.0.1")
                .WithConstructorArgument("apiUser", "user")
                .WithConstructorArgument("apiSecret", "secret");

            var s = kernel.Get<WeatherScenarioDescription>();

            // Create Scenario
            s.Create();

            // Get this scenario
            var scenario = kernel.Get<IScenario>();

            Assert.AreEqual("Météo", scenario.ScenarioName);
            scenario.Run();
        }
    }
}
