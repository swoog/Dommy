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

    public class AsyncHelper
    {
        private ILogger logger;

        private ISpeechLogger speechLogger;

        public AsyncHelper(ISpeechLogger speechLogger, ILogger logger)
        {
            this.speechLogger = speechLogger;
            this.logger = logger;
        }

        public void Wait(System.Action action)
        {
            Wait(() =>
            {
                action();
                return 0;
            });
        }

        public T Wait<T>(Func<T> action)
        {
            Contract.Requires(action != null);
            using (var cancelationToken = new CancellationTokenSource())
            {
                var ct = cancelationToken.Token;
                Task.Run(((Action)this.SayWait), ct);

                try
                {
                    this.logger.Debug("Begin action");
                    var result = action();
                    this.logger.Debug("End action");
                    return result;
                }
                finally
                {
                    cancelationToken.Cancel();
                }
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public T Retry<T>(int count, Func<T> action)
        {
            Contract.Requires(action != null);
            Contract.Requires(count >= 1);

            Exception error;
            do
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
                    this.logger.Error(ex, "Error retry ({0})", count);
                    error = ex;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
                count--;
            } while (count >= 0);

            throw error;
        }

        private void SayWait()
        {
            Thread.Sleep(TimeSpan.FromSeconds(1));

            if (!Task.Factory.CancellationToken.IsCancellationRequested &&
                !this.speechLogger.IgnoreRecognition)
            {

                this.logger.Debug("Say");
                var rechercheSentence = new string[]{
                        "Je recherche...",
                        "Je cherche...",
                        "Attend...",
                        "2 secondes...",
                        "3 secondes...",
                        "Recherche...",
                        "Patientez...",
                        "Encours...",
                        "Oui je recherche...",
                        "Je vais te dire cela dans quelques secondes.",
                    };

                this.speechLogger.Say(Actor.Dommy, StringHelper.Format(rechercheSentence));
            }
        }
    }
}
