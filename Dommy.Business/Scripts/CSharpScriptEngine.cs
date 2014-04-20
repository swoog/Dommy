//-----------------------------------------------------------------------
// <copyright file="CSharpScriptEngine.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scripts
{
    using System;

    /// <summary>
    /// Execute all C# scripts.
    /// </summary>
    public class CSharpScriptEngine : IScriptEngine
    {
        /// <summary>
        /// Gets file pattern of C# script name.
        /// </summary>
        public string FilePattern
        {
            get { return "*.csx"; }
        }

        /// <summary>
        /// Execute the C# script.
        /// </summary>
        /// <param name="file">File to parse and execute.</param>
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
