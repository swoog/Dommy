using Dommy.Business.Scripts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Dommy.Business.Test.Scripts
{
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
