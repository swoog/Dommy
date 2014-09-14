using Dommy.Business.Tools;

using Moq;
using Ninject.Extensions.Logging;
using System;
using System.Threading;

namespace Dommy.Business.Test.Tools
{
    using Xunit;

    public class AsyncHelperTest
    {
        [Fact]
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

        [Fact]
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

        [Fact]
        public void WaitTestLongRunTimeWithCallBeforeEnd()
        {
            var called = false;
            var end = false;
            var speechLogger = new Moq.Mock<ISpeechLogger>();
            speechLogger.Setup(s => s.Say(It.IsAny<Actor>(), It.IsAny<string>()))
                .Callback(() =>
                {
                    Assert.False(end);
                    called = true;
                });

            var helper = new AsyncHelper(speechLogger.Object, new Moq.Mock<ILogger>().Object);

            helper.Wait(() =>
            {
                Thread.Sleep(1002);
            });

            end = true;

            Assert.False(called);
        }
    }
}
