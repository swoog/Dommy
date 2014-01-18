//-----------------------------------------------------------------------
// <copyright file="SayResult.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Result
{
    using System;
    using System.Linq;

    /// <summary>
    /// Say text for result.
    /// </summary>
    public class SayResult : IResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SayResult"/> class.
        /// </summary>
        /// <param name="speech">Sentence to say.</param>
        public SayResult(string speech)
        {
            this.Speech = speech;
        }

        /// <summary>
        /// Gets speech to say.
        /// </summary>
        public string Speech { get; private set; }
    }
}
