
namespace Dommy.Business.Test.Scenarios
{
    using System.Collections.Generic;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ninject;

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
