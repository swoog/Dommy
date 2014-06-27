//-----------------------------------------------------------------------
// <copyright file="ISpeechToText.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Speech to text interface.
    /// </summary>
    public interface ISpeechToText
    {
        /// <summary>
        /// Initialize speech to text.
        /// </summary>
        void Init();

        /// <summary>
        /// Start speech to text.
        /// </summary>
        /// <param name="action">Action to execute when recognition is good.</param>
        void Start(Action<ISentence> action);

        /// <summary>
        /// Load grammar for speech to text recognition.
        /// </summary>
        /// <param name="grammar">Grammar to load.</param>
        void LoadGrammar(GrammarData grammar);

        /// <summary>
        /// Stop recognition.
        /// </summary>
        void Stop();

        /// <summary>
        /// Unload grammar from speech to text recognition.
        /// </summary>
        /// <param name="grammar">Grammar to unload.</param>
        void UnloadGrammar(GrammarData grammar);

        /// <summary>
        /// Gets a value indicating that the speech to text is active.
        /// </summary>
        bool IsActive { get; }
    }
}
