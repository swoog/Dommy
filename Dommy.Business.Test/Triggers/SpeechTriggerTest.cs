using Dommy.Business.Scenarios;
using Dommy.Business.Triggers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using Ninject.MockingKernel;
using Ninject.MockingKernel.Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Test.Triggers
{
    [TestClass]
    public class SpeechTriggerTest : BaseTest
    {
        [TestMethod]
        public void InitTest()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<IListener>().To<SpeechListener>()
                .WithConstructorArgument("confidence", 0.1);
            kernel.Bind<Engine>().ToSelf();

            var engine = kernel.Get<Engine>();
            var scenario = kernel.Get<IScenario>();

            var trigger = kernel.Get<SpeechTrigger>();

            trigger.Speech = new[] { "Sentence" };

            trigger.Init(engine, scenario);
        }
    }
}
