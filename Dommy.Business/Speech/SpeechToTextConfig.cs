//-----------------------------------------------------------------------
// <copyright file="SpeechToTextConfig.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Speech
{
    using Dommy.Business.Configs;
    using Ninject;
    using Ninject.Extensions.Conventions;

    /// <summary>
    /// Configuration of speech to text.
    /// </summary>
    public class SpeechToTextConfig : IConfig
    {
        /// <summary>
        /// Gets or sets culture.
        /// </summary>
        public string Culture { get; set; }

        /// <summary>
        /// Gets or sets confidence.
        /// </summary>
        public double Confidence { get; set; }

        /// <summary>
        /// Gets or sets sentence file.
        /// </summary>
        public string InfoSentenceFile { get; set; }

        /// <summary>
        /// Gets or sets implementation of speech to text.
        /// </summary>
        public string Implementation { get; set; }

        /// <summary>
        /// Create config.
        /// </summary>
        /// <param name="kernel">Ninject kernel.</param>
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
                .WithPropertyValue("SentenceLogFile", this.InfoSentenceFile);
        }
    }
}
