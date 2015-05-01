
namespace Dommy.Business.Test.Speech
{
    using Dommy.Business.Speech;

    using Xunit;

    public class SpeechToTextConfigTest : BaseTest
    {
        [Fact]
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
