using Dommy.Business.Scenarios;

namespace Dommy.Business.Test
{
    using Xunit;

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
