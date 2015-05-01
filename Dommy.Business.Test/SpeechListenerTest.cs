using Ninject;

namespace Dommy.Business.Test
{
    using Xunit;

    public class SpeechListenerTest : BaseTest
    {
        protected override Ninject.MockingKernel.Moq.MoqMockingKernel CreateKernel()
        {
            var kernel = base.CreateKernel();
            kernel.Bind<Engine>().ToSelf()
                .WithConstructorArgument("name", "Dommy");

            return kernel;
        }

        [Fact]
        public void SpeechRecognizedTest()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<SpeechListener>().ToSelf()
                .WithConstructorArgument("confidence", 0.1);

            var sentence = new Sentence { Text = "Sentence", Confidence = 0.1f, Words = new[] { "Sentence" }, WordsConfidence = new[] { 0.1f } };

            var listener = kernel.Get<SpeechListener>();
            listener.Init(kernel.Get<Engine>());

            dynamic listenerPrivate = new PrivateAccessor(listener);

            listenerPrivate.SpeechRecognized(sentence);
        }

        [Fact]
        public void StartTest()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<SpeechListener>().ToSelf()
                .WithConstructorArgument("confidence", 0.1);


            var listener = kernel.Get<SpeechListener>();
            listener.Init(kernel.Get<Engine>());

            listener.Start();
        }

        [Fact]
        public void CreateGrammarTest()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<SpeechListener>().ToSelf()
                .WithConstructorArgument("confidence", 0.1);


            var listener = kernel.Get<SpeechListener>();
            listener.Init(kernel.Get<Engine>());

            var gram = listener.CreateGrammar(s => { }, new[] { "Test" });

            Assert.NotNull(gram);
            Assert.NotNull(gram.Choices);
            Assert.Equal(1, gram.Choices.Count);
            Assert.NotNull(gram.Choices[0].Elements);
            Assert.Equal(1, gram.Choices[0].Elements.Count);
            Assert.Equal("Test", gram.Choices[0].Elements[0]);
        }

        [Fact]
        public void CreateGrammarTestWithPrefix()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<SpeechListener>().ToSelf()
                .WithConstructorArgument("confidence", 0.1);


            var listener = kernel.Get<SpeechListener>();
            listener.Init(kernel.Get<Engine>());

            var gram = listener.CreateGrammar(s => { }, new[] { "Test" }, true);

            Assert.NotNull(gram);
            Assert.NotNull(gram.Choices);
            Assert.Equal(2, gram.Choices.Count);
            Assert.NotNull(gram.Choices[0].Elements);
            Assert.Equal(1, gram.Choices[0].Elements.Count);
            Assert.Equal("Dommy", gram.Choices[0].Elements[0]);
            Assert.NotNull(gram.Choices[1].Elements);
            Assert.Equal(1, gram.Choices[1].Elements.Count);
            Assert.Equal("Test", gram.Choices[1].Elements[0]);
        }
    }
}
