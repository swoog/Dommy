using Dommy.Business.Scripts;

using System.Linq;

using Ninject;

namespace Dommy.Business.Test.Scripts
{
    using Xunit;

    public class ScriptEngineConfigTest
        : BaseTest
    {
        [Fact]
        public void CreateTest()
        {
            var kernel = this.CreateKernel();

            var config = new ScriptEngine.Config();

            config.Create(kernel);

            Assert.NotEqual(0, kernel.GetAll<IScriptEngine>().Count());
        }
    }
}
