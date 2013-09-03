using Dommy.Business.Scripts;
using Dommy.Business.Speech;
using Dommy.Business.Syntax;
using Ninject;
using Ninject.Extensions.Conventions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dommy.Business.Config
{
    public class Configure
    {
        private static IKernel kernel;

        public static void InitKernel(IKernel kernel)
        {
            Configure.kernel = kernel;
            kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IConfig>().BindSingleInterface());

            configs = kernel.GetAll<IConfig>().ToDictionary(c => c.GetType());
            configsName = configs.Keys.ToDictionary(c => c.FullName.Substring(c.Namespace.Length + 1).Replace("+", "."));
        }

        public static void Engine(string name = "Dommy")
        {
            kernel.Bind<Engine>()
                .ToSelf()
                .InSingletonScope()
                .WithPropertyValue("Name", name);
        }

        public static Configurator<SpeechToTextConfig> SpeechToText()
        {
            return Config<SpeechToTextConfig>();
        }

        public class TextToSpeechConfig : IConfig
        {
            public Gender Gender { get; set; }

            public string Culture { get; set; }


            public void Create(IKernel kernel)
            {
                kernel.Bind<ITextToSpeech>()
                    .To<MicrosoftTextToSpeech>()
                    .InSingletonScope()
                    .WithConstructorArgument("gender", this.Gender)
                    .WithConstructorArgument("culture", this.Culture);
            }
        }

        public static Configurator<TextToSpeechConfig> TextToSpeech()
        {
            return Config<TextToSpeechConfig>();
        }

        private static Dictionary<Type, IConfig> configs;
        private static Dictionary<string, Type> configsName;

        public static Configurator<T1> Config<T1>()
            where T1 : IConfig, new()
        {
            IConfig value;
            if (Configure.configs.TryGetValue(typeof(T1), out value))
            {
                return new Configurator<T1>((T1)value);
            }

            throw new NotImplementedException();
        }

        public static void Build()
        {
            foreach (var item in configs.Values)
            {
                item.Create(kernel);
            }
        }

        private static string configFile;

        public static void SaveConfig()
        {
            using (XmlWriter writer = XmlWriter.Create(Configure.configFile))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("Config", "Config");
                foreach (var config in configsName)
                {
                    var name = config.Key;
                    var type = config.Value;
                    var instance = configs[type];

                    writer.WriteStartElement(name, name);
                    WriteConfig(type, instance, writer);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private static void WriteConfig(Type type, IConfig instance, XmlWriter writer)
        {
            foreach (var item in type.GetProperties())
            {
                writer.WriteStartElement(item.Name, item.Name);
                writer.WriteString(String.Format("{0}", item.GetValue(instance)));
                writer.WriteEndElement();
            }
        }

        public static void LoadConfig(string configFile)
        {
            Configure.configFile = configFile;

            if (!File.Exists(configFile))
            {
                SaveConfig();
            }

            using (XmlReader reader = XmlReader.Create(configFile))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        var node = reader.Name;

                        try
                        {
                            if (configsName.ContainsKey(node))
                            {
                                var configName = configsName[node];

                                var config = configs[configName];

                                ReadConfig(node, config, reader);
                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                    }
                }
            }
        }

        private static void ReadConfig(string configName, IConfig config, XmlReader reader)
        {
            string propertyName = null;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && propertyName == null)
                {
                    propertyName = reader.Name;
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    var property = config.GetType().GetProperty(propertyName);
                    object val = null;

                    if (property.PropertyType.IsEnum)
                    {
                        val = Enum.Parse(property.PropertyType, reader.Value);
                    }
                    else
                    {
                        val = Convert.ChangeType(reader.Value, property.PropertyType);
                    }

                    property.SetValue(config, val);
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.Name == propertyName)
                    {
                        propertyName = null;
                    }
                    else if (reader.Name == configName)
                    {
                        return;
                    }
                }
            }
        }
    }
}
