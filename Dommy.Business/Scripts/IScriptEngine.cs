//-----------------------------------------------------------------------
// <copyright file="IScriptEngine.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scripts
{
    /// <summary>
    /// Interface to define a script engine.
    /// </summary>
    public interface IScriptEngine
    {
        /// <summary>
        /// Gets file name pattern.
        /// </summary>
        string FilePattern { get; }

        /// <summary>
        /// Execute script.
        /// </summary>
        /// <param name="file">File to execute.</param>
        void Execute(string file);
    }
}
