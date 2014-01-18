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
        public GrammarData()
        {
            this.Choices = new List<GrammarChoices>();
        }

        ///// <summary>
        ///// Gets or sets sentences.
        ///// </summary>
        //public IList<string> Sentences { get; set; }

        public IList<GrammarChoices> Choices { get; private set; }

        internal void Append(GrammarChoices choice)
        {
            this.Choices.Add(choice);
        }
    }
}
