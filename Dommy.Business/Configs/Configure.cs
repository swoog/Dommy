//-----------------------------------------------------------------------
// <copyright file="Configure.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Configs
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Dommy.Business.Speech;
    using Dommy.Business.Syntax;
    using Ninject;
    using Ninject.Extensions.Conventions;
    using System.Diagnostics.Contracts;
    using System.Globalization;

    /// <summary>
    /// Configure all elements in the project.
    /// </summary>
    public static class Configure
    {
        /// <summary>
        /// Config file.
        /// </summary>
        private static string configFile;

        /// <summary>
        /// Dictionary of configurations.
        /// </summary>
        private static Dictionary<Type, IConfig> configs;
        
        /// <summary>
        /// Dictionary of configurations type.
        /// </summary>
        private static Dictionary<string, Type> configsName;

        /// <summary>
        /// Ninject kernel.
        /// </summary>
        private static IKernel kernel;

        /// <summary>
        /// Initialize instance of the Ninject kernel used.
        /// </summary>
        /// <param name="kernel">Ninject kernel.</param>
        public static void InitKernel(IKernel kernel)
        {
            Configure.kernel = kernel;
            kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IConfig>().BindSingleInterface());

            configs = kernel.GetAll<IConfig>().ToDictionary(c => c.GetType());
            configsName = configs.Keys.ToDictionary(c => c.FullName.Substring(c.Namespace.Length + 1).Replace("+", "."));
        }

        /// <summary>
        /// Configure engine.
        /// </summary>
        /// <param name="name">Name of the engine. Default it's Dommy.</param>
        public static void Engine(string name = "Dommy")
        {
            kernel.Bind<Engine>()
                .ToSelf()
                .InSingletonScope()
                .WithConstructorArgument("name", name);
        }

        /// <summary>
        /// Configure SpeechToText.
        /// </summary>
        /// <returns>SpeechToText configurator.</returns>
        public static Configurator<SpeechToTextConfig> SpeechToText()
        {
            return Config<SpeechToTextConfig>();
        }

        /// <summary>
        /// Configure text to speech.
        /// </summary>
        /// <returns>Text to speech configurator.</returns>
        public static Configurator<TextToSpeechConfig> TextToSpeech()
        {
            return Config<TextToSpeechConfig>();
        }

        /// <summary>
        /// Configure another element of dommy.
        /// </summary>
        /// <typeparam name="T1">Type of the config.</typeparam>
        /// <returns>Configurator of the type.</returns>
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

        /// <summary>
        /// Build all configurations. Initialize Ninject kernel.
        /// </summary>
        public static void Build()
        {
            foreach (var item in configs.Values)
            {
                item.Create(kernel);
            }
        }

        /// <summary>
        /// Save actual configuration to XML file.
        /// </summary>
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

        /// <summary>
        /// Load configuration file.
        /// </summary>
        /// <param name="configFile">Configuration file name.</param>
        public static void LoadConfig(string configFile)
        {
            Contract.Requires(!string.IsNullOrEmpty(configFile));

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
                        catch 
                        {
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Write configuration element.
        /// </summary>
        /// <param name="type">Type of the config.</param>
        /// <param name="instance">Instance of the config.</param>
        /// <param name="writer">XML writer</param>
        private static void WriteConfig(Type type, IConfig instance, XmlWriter writer)
        {
            Contract.Requires(type != null);

            foreach (var item in type.GetProperties())
            {
                writer.WriteStartElement(item.Name, item.Name);
                writer.WriteString(string.Format(CultureInfo.InvariantCulture, "{0}", item.GetValue(instance)));
                writer.WriteEndElement();
            }
        }

        /// <summary>
        /// Read configuration element.
        /// </summary>
        /// <param name="configName">Config name.</param>
        /// <param name="config">Config instance.</param>
        /// <param name="reader">XML reader</param>
        private static void ReadConfig(string configName, IConfig config, XmlReader reader)
        {
            Contract.Requires(reader != null);

            string propertyName = string.Empty;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element && string.IsNullOrEmpty(propertyName))
                {
                    propertyName = reader.Name;
                }
                else if (reader.NodeType == XmlNodeType.Text && !string.IsNullOrEmpty(propertyName))
                {
                    var property = config.GetType().GetProperty(propertyName);
                    object val = null;

                    if (property.PropertyType.IsEnum)
                    {
                        val = Enum.Parse(property.PropertyType, reader.Value);
                    }
                    else
                    {
                        val = Convert.ChangeType(reader.Value, property.PropertyType, CultureInfo.InvariantCulture);
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

        /// <summary>
        /// Text to speech config class.
        /// </summary>
        public class TextToSpeechConfig : IConfig
        {
            /// <summary>
            /// Gets or sets text to speech gender.
            /// </summary>
            public Gender Gender { get; set; }

            /// <summary>
            /// Gets or sets text to speech culture.
            /// </summary>
            public string Culture { get; set; }

            /// <summary>
            /// Create ninject configuration.
            /// </summary>
            /// <param name="kernel">Kernel used for configuration.</param>
            public void Create(IKernel kernel)
            {
                kernel.Bind(c =>
                {
                    c.FromAssembliesMatching("*.dll")
                        .SelectAllClasses()
                        .InheritedFrom<ITextToSpeech>()
                        .BindUsingRegex("^ITextToSpeech$")
                        .Configure(conf => 
                            conf
                            .InSingletonScope()
                            .WithConstructorArgument("gender", this.Gender)
                            .WithConstructorArgument("culture", this.Culture));
                });
            }
        }
    }
}
