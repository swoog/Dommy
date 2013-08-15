using Dommy.Business.Scenarios;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business.Test
{
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
