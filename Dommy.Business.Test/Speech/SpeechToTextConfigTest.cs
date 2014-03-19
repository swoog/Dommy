
namespace Dommy.Business.Test.Speech
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Dommy.Business.Speech;

    [TestClass]
    public class SpeechToTextConfigTest : BaseTest
    {
        [TestMethod]
        public void CreateTest()
        {
            var kernel = this.CreateKernel();

            var speechToTextConfig = new SpeechToTextConfig();

            speechToTextConfig.Confidence = 0.6;
            speechToTextConfig.Culture = "fr-FR";
            speechToTextConfig.InfoSentenceFile = "c:\fileName.txt";

            speechToTextConfig.Create(kernel);
        }
    }
}
