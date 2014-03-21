
namespace Dommy.Business.Test.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Dommy.Business.Configs;

    [TestClass]
    public class ConfigureTest
        : BaseTest
    {
        [TestMethod]
        public void LoadSaveConfigTest()
        {
            Configure.InitKernel(this.CreateKernel());
            Configure.Config<ConfigTest>();

            Configure.LoadConfig("config.xml");

            Configure.SaveConfig();
        }

        public class ConfigTest : IConfig
        {
            public void Create(Ninject.IKernel kernel)
            {
                throw new NotImplementedException();
            }
        }
    }
}
