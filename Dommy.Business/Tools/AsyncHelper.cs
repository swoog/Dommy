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
        private SpeechLogger speechLogger;

        public AsyncHelper(SpeechLogger speechLogger)
        {
            this.speechLogger = speechLogger;
        }

        public void Wait(System.Action action)
        {
            int i = Wait(() =>
            {
                action();
                return 0;
            });
        }

        public T Wait<T>(Func<T> action)
        {
            bool isEnd = false;
            Task.Factory.StartNew(() =>
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));

                if (!isEnd)
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

                    this.speechLogger.ErrorRecognition(Actor.Dommy, StringHelper.Format(rechercheSentence));
                }
            });

            try
            {
                var result = action();
                return result;
            }
            finally
            {
                isEnd = true;
            }
        }

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
    }
}
