//-----------------------------------------------------------------------
// <copyright file="EmailAction.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Action
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Model;

    /// <summary>
    /// Email action.
    /// </summary>
    public class EmailAction : BaseAction<ActionData>
    {
        /// <summary>
        /// Gets all matched sentences with the name engine.
        /// </summary>
        public override IList<string> Sentences
        {
            get
            {
                return new[]
                {
                    "Combien j'ai d'e-mail",
                    "Combien d'e-mail",
                    "Combien d'e-mail non lu",
                };
            }
        }

        /// <summary>
        /// Run action match.
        /// </summary>
        /// <param name="sentence">Information about sentence match.</param>
        /// <returns>Result to execute.</returns>
        public override IResult RunAction(ISentence sentence)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Run action without sentence match.
        /// </summary>
        /// <returns>Result to execute.</returns>
        public override IResult RunAction()
        {
            throw new NotImplementedException();
        }
    }
}
