//-----------------------------------------------------------------------
// <copyright file="PrecisionResult.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Result
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Result of action to call precision.
    /// </summary>
    public class PrecisionResult : SayResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrecisionResult"/> class.
        /// </summary>
        /// <param name="speech">Question sentence.</param>
        /// <param name="sentenceActions">Accepted sentences.</param>
        public PrecisionResult(string speech, IList<SentenceAction> sentenceActions)
            : base(speech)
        {
            this.SentenceActions = sentenceActions;
        }

        /// <summary>
        /// Gets accepted sentences.
        /// </summary>
        public IList<SentenceAction> SentenceActions { get; private set; }

        /// <summary>
        /// Accepted sentence.
        /// </summary>
        public class SentenceAction
        {
            /// <summary>
            /// Gets or sets unique key.
            /// </summary>
            public string UniqueKey { get; set; }

            /// <summary>
            /// Gets or sets accepted sentences.
            /// </summary>
            public IList<string> Sentences { get; set; }

            /// <summary>
            /// Gets or sets action to execute.
            /// </summary>
            public Func<ISentence, IResult> Action { get; set; }
        }
    }
}
