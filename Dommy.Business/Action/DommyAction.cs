//-----------------------------------------------------------------------
// <copyright file="DommyAction.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Action
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Dommy.Business.Result;
    using Dommy.Business.Tools;
    using Dommy.Model;
    using Ninject;

    /// <summary>
    /// Make Dommy focus on your next sentence.
    /// </summary>
    public class DommyAction : BaseAction<ActionData>
    {
        /// <summary>
        /// Ninject kernel.
        /// </summary>
        private IKernel kernel;

        /// <summary>
        /// All sentence actions.
        /// </summary>
        private List<PrecisionResult.SentenceAction> sentenceActions = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DommyAction"/> class.
        /// </summary>
        /// <param name="kernel">Ninject kernel.</param>
        public DommyAction(IKernel kernel)
        {
            this.kernel = kernel;
        }

        /// <summary>
        /// Gets all matched sentences without the name engine.
        /// </summary>
        public override IList<string> SentencesNoPrefixName
        {
            get { return new string[] { "Dommy" }; }
        }

        /// <summary>
        /// Gets all matched sentences with the name engine.
        /// </summary>
        public override IList<string> Sentences
        {
            get { return new string[0]; }
        }

        /// <summary>
        /// Run action match.
        /// </summary>
        /// <param name="sentence">Information about sentence match.</param>
        /// <returns>Result to execute.</returns>
        public override IResult RunAction(ISentence sentence)
        {
            if (sentence.Confidence > 0.85)
            {
                return this.RunAction();
            }

            return new NoneResult();
        }

        /// <summary>
        /// Run action without sentence match.
        /// </summary>
        /// <returns>Result to execute.</returns>
        public override IResult RunAction()
        {
            this.InitGrammar();

            var responses = new[]
            {
                "Oui ?",
                "Que puis-je faire ?",
                "Que veux-tu ?",
                "Comment puis-je t'aider ?",
                "Comment je peux t'aider ?",
                "Qu'est ce que je peut faire ?",
                "A tes ordres !",
                "Quoi ?",
            };

            return new PrecisionResult(StringHelper.Format(responses), this.sentenceActions);
        }

        /// <summary>
        /// Initialized all short grammar.
        /// </summary>
        private void InitGrammar()
        {
            if (this.sentenceActions == null)
            {
                var engine = this.kernel.Get<Engine>();

                this.sentenceActions = new List<PrecisionResult.SentenceAction>();
                foreach (var action in engine.Actions)
                {
                    var sentences = action.Sentences;

                    this.sentenceActions.Add(new PrecisionResult.SentenceAction
                    {
                        Sentences = action.Sentences,
                        Action = sentence =>
                        {
                            return action.RunAction(sentence);
                        },
                        UniqueKey = Guid.NewGuid().ToString(),
                    });
                }
            }
        }
    }
}
