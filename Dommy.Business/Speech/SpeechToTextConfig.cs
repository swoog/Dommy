using Dommy.Business.Config;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Speech
{
    public class SpeechToTextConfig : IConfig
    {
        private string culture;
        private double confidence;
        private string infoSentenceFile;
        public SpeechToTextConfig Culture(string culture)
        {
            this.culture = culture;
            return this;
        }

        public SpeechToTextConfig Confidence(double confidence)
        {
            this.confidence = confidence;
            return this;
        }

        public void Create(IKernel kernel)
        {
            kernel.Bind<ISpeechToText>().To<KinectSpeechToText>().InSingletonScope()
                .WithConstructorArgument("culture", this.culture);
            kernel.Bind<SpeechListener>().ToSelf()
                .WithConstructorArgument("confidence", this.confidence)
                .WithPropertyValue("SentenceLogFile", this.infoSentenceFile)
                ;

        }

        public SpeechToTextConfig InfoSentenceFile(string infoSentenceFile)
        {
            this.infoSentenceFile = infoSentenceFile;
            return this;
        }
    }
}
