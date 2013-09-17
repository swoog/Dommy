using Dommy.Business.Tools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dommy.Business.Test.Tools
{
    [TestClass]
    public class AsyncHelperTest
    {
        [TestMethod]
        public void WaitTest()
        {
            var speechLogger = new Moq.Mock<ISpeechLogger>();
            speechLogger.Setup(s => s.Say(It.IsAny<Actor>(), It.IsAny<string>()))
                .Throws(new Exception("Call to this method is deny"));

            var helper = new AsyncHelper(speechLogger.Object);

            helper.Wait(() =>
            {
                Thread.Sleep(500);
            });
        }

        [TestMethod]
        public void WaitTest_LongRunTime()
        {
            var speechLogger = new Moq.Mock<ISpeechLogger>();
            speechLogger.Setup(s => s.Say(It.IsAny<Actor>(), It.IsAny<string>()))
                .Verifiable();

            var helper = new AsyncHelper(speechLogger.Object);

            helper.Wait(() =>
            {
                Thread.Sleep(1100);
            });

            speechLogger.Verify();
        }
    }
}
