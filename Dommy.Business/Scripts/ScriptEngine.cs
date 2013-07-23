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
        public string ScriptDirectory { get; set; }

        private IList<IScriptEngine> scriptEngines;
        private SpeechLogger speechLogger;
        private ILogger logger;

        public ScriptEngine(IList<IScriptEngine> scriptEngines, SpeechLogger speechLogger, ILogger logger)
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
