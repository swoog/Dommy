using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Dommy.Business.Test
{
    [TestClass]
    public class SpeechListenerTest : BaseTest
    {
        [TestMethod]
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
    }
}
