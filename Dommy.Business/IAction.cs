//-----------------------------------------------------------------------
// <copyright file="IAction.cs" company="Microsoft">
//     Copyright (c) agaltier, Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Dommy.Model;

    /// <summary>
    /// Interface of an action.
    /// </summary>
    [Obsolete]
    public interface IAction
    {
        /// <summary>
        /// Get the sentences who make the action.
        /// </summary>
        IList<string> Sentences { get; }

        /// <summary>
        /// Get the sentences who make the action without prefix.
        /// </summary>
        IList<string> SentencesNoPrefixName { get; }

        /// <summary>
        /// Run the action.
        /// </summary>
        /// <param name="sentence"></param>
        IResult RunAction(ISentence sentence);

        ActionData Data { get; }

        IResult RunAction(ActionData data);
    }
}
