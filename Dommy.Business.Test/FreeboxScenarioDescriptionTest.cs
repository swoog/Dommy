
namespace Dommy.Business.Test
{
    using Xunit;
    using Dommy.Business.Scenarios;

    public class FreeboxScenarioDescriptionTest
    {
        [Fact]
        public void FreeboxVersion()
        {
            var f = new FreeboxScenarioDescription(null);

            f.FreeboxVersion();
        }
    }
}
