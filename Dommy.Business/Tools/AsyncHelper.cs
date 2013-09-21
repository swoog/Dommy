using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dommy.Business.Tools
{
    public class AsyncHelper
    {
        private ISpeechLogger speechLogger;

        public AsyncHelper(ISpeechLogger speechLogger)
        {
            this.speechLogger = speechLogger;
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
            var cancelationToken = new CancellationTokenSource();
            var ct = cancelationToken.Token;
            var t = Task.Run(((Action)this.SayWait), ct);

            try
            {
                var result = action();
                return result;
            }
            finally
            {
                cancelationToken.Cancel();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public T Retry<T>(int count, Func<T> action)
        {
            Exception error;
            do
            {
                try
                {
                    return action();
                }
                catch (Exception ex)
                {
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

            if (!Task.Factory.CancellationToken.IsCancellationRequested)
            {
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
