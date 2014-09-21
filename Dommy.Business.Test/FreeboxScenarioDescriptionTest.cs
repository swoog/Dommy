
namespace Dommy.Business.Test
{
    using Dommy.Business.Scenarios;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FreeboxScenarioDescriptionTest
    {
        [TestMethod]
        public void FreeboxVersion()
        {
            var f = new FreeboxScenarioDescription(null);

            f.FreeboxVersion();
        }
    }
}
