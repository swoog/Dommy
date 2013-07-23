using Dommy.Business.Action;
using Dommy.Business.Speech;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Config
{
    public class Configure
    {
        private static IKernel kernel;

        public static void InitKernel(IKernel kernel)
        {
            Configure.kernel = kernel;
        }

        public static void Engine(string name = "Dommy")
        {
            kernel.Bind<Engine>()
                .ToSelf()
                .InSingletonScope()
                .WithPropertyValue("Name", name);
        }

        public static SpeechToTextConfig SpeechToText()
        {
            return Config<SpeechToTextConfig>();
        }

        public class TextToSpeechConfig : IConfig
        {
            private Gender gender;
            public TextToSpeechConfig Gender(Gender gender)
            {
                this.gender = gender;
                return this;
            }

            private string culture;
            public TextToSpeechConfig Culture(string culture)
            {
                this.culture = culture;
                return this;
            }

            public void Create(IKernel kernel)
            {
                kernel.Bind<ITextToSpeech>()
                    .To<MicrosoftTextToSpeech>()
                    .InSingletonScope()
                    .WithConstructorArgument("gender", this.gender)
                    .WithConstructorArgument("culture", this.culture);
            }
        }

        public static RestListener.Config RestListener()
        {
            return Config<RestListener.Config>();
        }

        public static TextToSpeechConfig TextToSpeech()
        {
            return Config<TextToSpeechConfig>();
        }

        private static Dictionary<Type, IConfig> configs = new Dictionary<Type, IConfig>();

        public static T1 Config<T1>()
            where T1 : IConfig, new()
        {
            IConfig value ;
            if (Configure.configs.TryGetValue(typeof(T1), out value))
            {
                return (T1)value;
            }

            value = new T1();
            Configure.configs.Add(typeof(T1), value);
            return (T1)value;
        }

        public static void Build()
        {
            //var scriptEngine = kernel.Get<Scripts.ScriptEngine>(new );

            //scriptEngine.Execute();

            foreach (var item in configs.Values)
            {
                item.Create(kernel);
            }
        }
    }
}
