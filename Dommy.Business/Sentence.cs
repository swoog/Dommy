//-----------------------------------------------------------------------
// <copyright file="Sentence.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System.Collections.Generic;

    /// <summary>
    /// Describe a recognized sentence.
    /// </summary>
    public class Sentence : ISentence
    {
        /// <summary>
        /// Gets or sets recognized words.
        /// </summary>
        public ICollection<string> Words { get; set; }

        /// <summary>
        /// Gets or sets recognized sentence.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets confidence.
        /// </summary>
        public float Confidence { get; set; }

        /// <summary>
        /// Gets or sets confidence for each word.
        /// </summary>
        public ICollection<float> WordsConfidence { get; set; }
    }
}
