using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Scripts
{
    public class CSharpScriptEngine : IScriptEngine
    {
        public string FilePattern
        {
            get { return "*.csx"; }
        }

        public void Execute(string file)
        {
            var engine = new Roslyn.Scripting.CSharp.ScriptEngine();
            engine.AddReference("System");
            engine.AddReference("System.Core");
            engine.AddReference("System.ServiceModel");
            engine.AddReference("System.Xml");

            var session = engine.CreateSession(this);
            session.SetReferenceSearchPaths(Environment.CurrentDirectory);
            session.ExecuteFile(file);
        }
    }
}
