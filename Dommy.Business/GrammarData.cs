//-----------------------------------------------------------------------
// <copyright file="GrammarData.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System.Collections.Generic;

    /// <summary>
    /// Grammar data for SpeechToText.
    /// </summary>
    public class GrammarData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarData"/> class.
        /// </summary>
        public GrammarData()
        {
            this.Choices = new List<GrammarChoices>();
        }

        /// <summary>
        /// Gets grammar choices.
        /// </summary>
        public IList<GrammarChoices> Choices { get; private set; }

        /// <summary>
        /// Add new grammar choice.
        /// </summary>
        /// <param name="choice">Grammar choice.</param>
        internal void Append(GrammarChoices choice)
        {
            this.Choices.Add(choice);
        }
    }
}
