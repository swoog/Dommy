namespace Dommy.Business.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BootstrapTests
    {
        [TestMethod]
        public void Should_load_configuration_when_run()
        {
            Bootstrap.Run();
        }
    }
}