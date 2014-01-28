//-----------------------------------------------------------------------
// <copyright file="GrammarChoices.cs" company="TrollCorp">
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
    /// Represent grammar sentences choices.
    /// </summary>
    public class GrammarChoices
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GrammarChoices"/> class.
        /// </summary>
        public GrammarChoices()
        {
            this.Elements = new List<string>();
        }

        /// <summary>
        /// Gets sentences.
        /// </summary>
        public IList<string> Elements { get; private set; }

        /// <summary>
        /// Add sentences.
        /// </summary>
        /// <param name="element">String sentence.</param>
        internal void Add(string element)
        {
            this.Elements.Add(element);
        }
    }
}
