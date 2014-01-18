//-----------------------------------------------------------------------
// <copyright file="ITextToSpeech.cs" company="TrollCorp">
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
    /// Text to speech interface.
    /// </summary>
    public interface ITextToSpeech
    {
        /// <summary>
        /// Speak the text.
        /// </summary>
        /// <param name="text">Speaking text.</param>
        void Speak(string text);
    }
}
