//-----------------------------------------------------------------------
// <copyright file="ScriptEngine.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scripts
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.IO;
    using Dommy.Business.Configs;

    using JetBrains.Annotations;

    using Ninject.Extensions.Conventions;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Execute all scenarios scripts.
    /// </summary>
    public class ScriptEngine
    {
        /// <summary>
        /// All scripts engines.
        /// </summary>
        private readonly IList<IScriptEngine> scriptEngines;

        /// <summary>
        /// Speech logger.
        /// </summary>
        private readonly ISpeechLogger speechLogger;

        /// <summary>
        /// Errors logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptEngine"/> class.
        /// </summary>
        /// <param name="scriptEngines">All script engine.</param>
        /// <param name="speechLogger">Speech logger.</param>
        /// <param name="logger">Errors logger.</param>
        public ScriptEngine(IList<IScriptEngine> scriptEngines, ISpeechLogger speechLogger, ILogger logger)
        {
            this.scriptEngines = scriptEngines;
            this.speechLogger = speechLogger;
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets scripts directory.
        /// </summary>
        [UsedImplicitly]
        public string ScriptDirectory { get; set; }

        /// <summary>
        /// Execute all found scripts.
        /// </summary>
        public void Execute()
        {
            this.ExecuteDirectory(".");
            this.ExecuteDirectory(this.ScriptDirectory);
        }

        /// <summary>
        /// Execute all found scripts in directory.
        /// </summary>
        /// <param name="directory">Directory to search.</param>
        private void ExecuteDirectory(string directory)
        {
            Contract.Requires(!string.IsNullOrEmpty(directory));

            if (!Directory.Exists(directory))
            {
                try
                {
                    Directory.CreateDirectory(directory);
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, "Can't create directory {0}", directory);
                }

                return;
            }

            this.logger.Info("Scripts scan : {0}", new DirectoryInfo(directory).FullName);

            foreach (var scriptEngine in this.scriptEngines)
            {
                foreach (var file in Directory.GetFiles(directory, scriptEngine.FilePattern))
                {
                    string scriptName = Path.GetFileNameWithoutExtension(file);
                    try
                    {
                        this.logger.Info("Scripts execute : {0}", scriptName);
                        scriptEngine.Execute(file);
                    }
                    catch (Exception ex)
                    {
                        this.speechLogger.Say(Actor.Dommy, "Erreur dans le script " + scriptName);
                        this.logger.Error(ex, "Erreur dans le script", scriptName);
                    }
                }
            }
        }

        /// <summary>
        /// Configuration of script engine.
        /// </summary>
        public class Config : IConfig
        {
            /// <summary>
            /// Gets or sets the scripts directory.
            /// </summary>
            [UsedImplicitly]
            public string ScriptDirectory { get; set; }

            /// <summary>
            /// Create script engine Ninject configuration.
            /// </summary>
            /// <param name="kernel">Ninject kernel.</param>
            public void Create(Ninject.IKernel kernel)
            {
                kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IScriptEngine>().BindSingleInterface());

                kernel.Bind<ScriptEngine>().ToSelf()
                    .WithPropertyValue("ScriptDirectory", this.ScriptDirectory);
            }
        }
    }
}
