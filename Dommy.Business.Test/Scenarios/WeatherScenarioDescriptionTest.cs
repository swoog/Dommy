using Dommy.Business.Scenarios;

using System.Collections.Generic;

using Ninject;

using Dommy.Business.Tools;
using Dommy.Business.Syntax;

namespace Dommy.Business.Test.Scenarios
{
    using Xunit;

    public class WeatherScenarioDescriptionTest : BaseTest
    {
        [Fact]
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

            Assert.Equal("Météo", scenario.ScenarioName);
            scenario.Run();
        }
    }
}
