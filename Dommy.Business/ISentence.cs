//-----------------------------------------------------------------------
// <copyright file="ISentence.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System.Collections.Generic;

    /// <summary>
    /// Information of a recognized sentence.
    /// </summary>
    public interface ISentence
    {
        /// <summary>
        /// Gets words of the sentence.
        /// </summary>
        ICollection<string> Words { get; }

        /// <summary>
        /// Gets confidence of each words.
        /// </summary>
        ICollection<float> WordsConfidence { get; }

        /// <summary>
        /// Gets recognized sentence.
        /// </summary>
        string Text { get; }

        /// <summary>
        /// Gets confidence of the sentences.
        /// </summary>
        float Confidence { get; }
    }
}
