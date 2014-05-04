
namespace Dommy.Business.Test.Scripts
{
    using System.Linq;
    using Dommy.Business.Scripts;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Ninject;

    [TestClass]
    public class ScriptEngineConfigTest
        : BaseTest
    {
        [TestMethod]
        public void CreateTest()
        {
            var kernel = this.CreateKernel();

            var config = new ScriptEngine.Config();

            config.Create(kernel);

            Assert.AreNotEqual(0, kernel.GetAll<IScriptEngine>().Count());
        }
    }
}
