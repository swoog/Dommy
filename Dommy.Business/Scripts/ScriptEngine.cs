using Dommy.Business.Config;
using Ninject;
using Ninject.Extensions.Conventions;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Scripts
{
    public class ScriptEngine
    {
        public class Config : IConfig
        {
            public string ScriptDirectory { get; set; }

            public void Create(Ninject.IKernel kernel)
            {
                kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IScriptEngine>().BindSingleInterface());

                kernel.Bind<ScriptEngine>().ToSelf()
                    .WithPropertyValue("ScriptDirectory", this.ScriptDirectory)
                    ;
            }
        }

        public string ScriptDirectory { get; set; }

        private IList<IScriptEngine> scriptEngines;
        private ISpeechLogger speechLogger;
        private ILogger logger;

        public ScriptEngine(IList<IScriptEngine> scriptEngines, ISpeechLogger speechLogger, ILogger logger)
        {
            this.scriptEngines = scriptEngines;
            this.speechLogger = speechLogger;
            this.logger = logger;
        }

        public void Execute()
        {
            ExecuteDirectory(".");
            ExecuteDirectory(this.ScriptDirectory);
        }

        private void ExecuteDirectory(string directory)
        {
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
    }
}
