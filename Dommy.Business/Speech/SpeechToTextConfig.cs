using Dommy.Business.Config;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Extensions.Conventions;

namespace Dommy.Business.Speech
{
    public class SpeechToTextConfig : IConfig
    {
        public string Culture { get; set; }
        public double Confidence { get; set; }
        public string InfoSentenceFile { get; set; }

        public string Implementation { get; set; }

        public void Create(IKernel kernel)
        {
            kernel.Bind(c =>
            {
                c.FromAssembliesMatching("*.dll")
                    .SelectAllClasses()
                    .InheritedFrom<ISpeechToText>()
                    .BindUsingRegex("^ISpeechToText$")
                    .Configure(conf => conf
                        .InSingletonScope()
                        .WithConstructorArgument("culture", this.Culture));
            });

            kernel.Bind<IListener>().To<SpeechListener>()
                .WithConstructorArgument("confidence", this.Confidence)
                .WithPropertyValue("SentenceLogFile", this.InfoSentenceFile)
                ;
        }
    }
}
