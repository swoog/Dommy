﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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

        [TestMethod]
        public void StartTest()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<SpeechListener>().ToSelf()
                .WithConstructorArgument("confidence", 0.1);


            var listener = kernel.Get<SpeechListener>();
            listener.Init(kernel.Get<Engine>());

            listener.Start();
        }

        [TestMethod]
        public void CreateGrammarTest()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<SpeechListener>().ToSelf()
                .WithConstructorArgument("confidence", 0.1);


            var listener = kernel.Get<SpeechListener>();
            listener.Init(kernel.Get<Engine>());

            var gram = listener.CreateGrammar(s => { }, new[] { "Test" });

            Assert.IsNotNull(gram);
            Assert.IsNotNull(gram.Choices);
            Assert.AreEqual(1, gram.Choices.Count);
            Assert.IsNotNull(gram.Choices[0].Elements);
            Assert.AreEqual(1, gram.Choices[0].Elements.Count);
            Assert.AreEqual("Test", gram.Choices[0].Elements[0]);
        }

        [TestMethod]
        public void CreateGrammarTest_WithPrefix()
        {
            var kernel = this.CreateKernel();

            kernel.Bind<Engine>().ToSelf()
                .WithConstructorArgument("name", "Dommy");

            kernel.Bind<SpeechListener>().ToSelf()
                .WithConstructorArgument("confidence", 0.1);


            var listener = kernel.Get<SpeechListener>();
            listener.Init(kernel.Get<Engine>());

            var gram = listener.CreateGrammar(s => { }, new[] { "Test" }, true);

            Assert.IsNotNull(gram);
            Assert.IsNotNull(gram.Choices);
            Assert.AreEqual(2, gram.Choices.Count);
            Assert.IsNotNull(gram.Choices[0].Elements);
            Assert.AreEqual(1, gram.Choices[0].Elements.Count);
            Assert.AreEqual("Dommy", gram.Choices[0].Elements[0]);
            Assert.IsNotNull(gram.Choices[1].Elements);
            Assert.AreEqual(1, gram.Choices[1].Elements.Count);
            Assert.AreEqual("Test", gram.Choices[1].Elements[0]);
        }
    }
}
