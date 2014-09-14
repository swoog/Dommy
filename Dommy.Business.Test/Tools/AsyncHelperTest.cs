
namespace Dommy.Business.Test.Tools
{
    using System;
    using System.Threading;
    using Dommy.Business.Tools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ninject.Extensions.Logging;

    [TestClass]
    public class AsyncHelperTest
    {
        [TestMethod]
        public void WaitTest()
        {
            var speechLogger = new Moq.Mock<ISpeechLogger>();
            speechLogger.Setup(s => s.Say(It.IsAny<Actor>(), It.IsAny<string>()))
                .Throws(new UnauthorizedAccessException("Call to this method is deny"));

            var helper = new AsyncHelper(speechLogger.Object, new Moq.Mock<ILogger>().Object);

            helper.Wait(() =>
            {
                Thread.Sleep(500);
            });
        }

        [TestMethod]
        public void WaitTestLongRunTime()
        {
            var speechLogger = new Moq.Mock<ISpeechLogger>();
            speechLogger.Setup(s => s.Say(It.IsAny<Actor>(), It.IsAny<string>()))
                .Verifiable();

            var helper = new AsyncHelper(speechLogger.Object, new Moq.Mock<ILogger>().Object);

            helper.Wait(() =>
            {
                Thread.Sleep(1100);
            });

            speechLogger.Verify();
        }

        [TestMethod]
        public void WaitTestLongRunTimeWithCallBeforeEnd()
        {
            var called = false;
            var end = false;
            var speechLogger = new Moq.Mock<ISpeechLogger>();
            speechLogger.Setup(s => s.Say(It.IsAny<Actor>(), It.IsAny<string>()))
                .Callback(() =>
                {
                    Assert.IsFalse(end);
                    called = true;
                });

            var helper = new AsyncHelper(speechLogger.Object, new Moq.Mock<ILogger>().Object);

            helper.Wait(() =>
            {
                Thread.Sleep(1002);
            });

            end = true;

            Assert.IsFalse(called);
        }
    }
}
