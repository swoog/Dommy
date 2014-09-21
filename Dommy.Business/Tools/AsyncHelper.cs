//-----------------------------------------------------------------------
// <copyright file="AsyncHelper.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Tools
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Threading;
    using System.Threading.Tasks;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Asynchronous helper.
    /// </summary>
    public class AsyncHelper
    {
        /// <summary>
        /// Errors logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Speech logger.
        /// </summary>
        private readonly ISpeechLogger speechLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncHelper"/> class.
        /// </summary>
        /// <param name="speechLogger">Speech logger.</param>
        /// <param name="logger">Errors logger.</param>
        public AsyncHelper(ISpeechLogger speechLogger, ILogger logger)
        {
            this.speechLogger = speechLogger;
            this.logger = logger;
        }

        /// <summary>
        /// Decorate the code with the wait pattern.
        /// </summary>
        /// <param name="action">Code to wait.</param>
        public void Wait(Action action)
        {
            this.Wait(() =>
                {
                    action();
                    return 0;
                });
        }

        /// <summary>
        /// Decorate the code with the wait pattern.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="action">Code to wait.</param>
        private void Wait<T>(Func<T> action)
        {
            Contract.Requires(action != null);
            using (var cancelationToken = new CancellationTokenSource())
            {
                var ct = cancelationToken.Token;
                Task.Run((Action)this.SayWait, ct);

                try
                {
                    this.logger.Debug("Begin action");
                    action();
                    this.logger.Debug("End action");
                    return;
                }
                finally
                {
                    cancelationToken.Cancel();
                }
            }
        }

        /// <summary>
        /// Method used to say wait sentence after 1 second.
        /// </summary>
        private void SayWait()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (!Task.Factory.CancellationToken.IsCancellationRequested 
                && !this.speechLogger.IgnoreRecognition)
            {
                this.logger.Debug("Say");
                var rechercheSentence = new[]
                {
                    "Je recherche...",
                    "Je cherche...",
                    "Attend...",
                    "2 secondes...",
                    "3 secondes...",
                    "Recherche...",
                    "Patientez...",
                    "Encours...",
                    "Oui je recherche...",
                    "Je vais te dire cela dans quelques secondes."
                };

                this.speechLogger.Say(Actor.Dommy, StringHelper.Format(rechercheSentence));
            }
        }
    }
}
