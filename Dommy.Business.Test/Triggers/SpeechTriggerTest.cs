using Dommy.Business.Scenarios;
using Dommy.Business.Triggers;

using Ninject;

namespace Dommy.Business.Test.Triggers
{
    using Xunit;

    public class SpeechTriggerTest : BaseTest
    {
        protected override Ninject.MockingKernel.Moq.MoqMockingKernel CreateKernel()
        {
            var kernel = base.CreateKernel();
            kernel.Bind<Engine>().ToSelf()
                .WithConstructorArgument("name", "Dommy");

            return kernel;
        }

        [Fact]
        public void InitTest()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<IListener>().To<SpeechListener>()
                .WithConstructorArgument("confidence", 0.1);

            var engine = kernel.Get<Engine>();
            var scenario = kernel.Get<IScenario>();

            var trigger = kernel.Get<SpeechTrigger>();

            trigger.Speech = new[] { "Sentence" };

            trigger.Init(engine, scenario);
        }
    }
}
