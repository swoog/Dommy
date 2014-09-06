//-----------------------------------------------------------------------
// <copyright file="ISpeechTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Triggers
{
    using Dommy.Business.Syntax;

    /// <summary>
    /// Trigger syntax for speech.
    /// </summary>
    public interface ISpeechTriggerSyntax : IExtendSyntax
    {
        /// <summary>
        /// Create a scenario with speech trigger for sentences without prefix.
        /// </summary>
        /// <param name="sentences">Sentences speech trigger.</param>
        /// <returns>Trigger scenario syntax.</returns>
        ITriggerScenarioSyntax NoPrefixSpeechTrigger(params string[] sentences);

        /// <summary>
        /// Create a scenario with speech trigger for sentences with prefix.
        /// </summary>
        /// <param name="sentences">Sentences speech trigger.</param>
        /// <returns>Trigger scenario syntax.</returns>
        ITriggerScenarioSyntax SpeechTrigger(params string[] sentences);

        /// <summary>
        /// Create a scenario with speech trigger for sentences with prefix and specific confidence.
        /// </summary>
        /// <param name="confidence">Speech confidence.</param>
        /// <param name="sentences">Sentences speech trigger.</param>
        /// <returns>Trigger scenario syntax.</returns>
        ITriggerScenarioSyntax SpeechTrigger(double confidence, params string[] sentences);
    }
}
