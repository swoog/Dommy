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
        public string Culture {get;set;}
        public double Confidence { get; set; }
        public string InfoSentenceFile { get; set; }

        public void Create(IKernel kernel)
        {
            kernel.Bind<ISpeechToText>().To<KinectSpeechToText>().InSingletonScope()
                .WithConstructorArgument("culture", this.Culture);
            kernel.Bind<SpeechListener>().ToSelf()
                .WithConstructorArgument("confidence", this.Confidence)
                .WithPropertyValue("SentenceLogFile", this.InfoSentenceFile)
                ;
        }
    }
}
