using Dommy.Business.Scenarios;
using Dommy.Business.Syntax;
using Dommy.Business.Triggers;
using Dommy.Business.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Scenarios
{
    public class TvScenarioDescription : IScenarioDescription
    {
        private ITvHelper tvHelper;

        public TvScenarioDescription(ITvHelper tvHelper)
        {
            this.tvHelper = tvHelper;
        }

        public void Create()
        {
            for (int i = 0; i < Channel.ChannelsCount; i++)
            {
                var sentence = new[]{
                    "Met la {Chaine}",
                    "Met la chaine {Chaine}",
                    "La {Chaine}",
                };

                var channel = Channel.GetChannel(i);

                var channels = (from s in sentence
                                select StringHelper.Format(s, new { Chaine = channel.NumberToString })).ToArray();

                Scenario.Create(StringHelper.Format("TV {Name}", channel))
                    .SpeechTrigger(0.4, channels)
                    .Action(() =>
                    {
                        this.tvHelper.Canal(channel.Number);
                        return true;
                    })
                    .Start();
            }

            Scenario.Create("TV Pause")
                .SpeechTrigger("Met pause", "pause")
                .TvCommand(TvCommand.Pause)
                .Start();

            Scenario.Create("TV Stop")
                .SpeechTrigger("Met stop", "stop")
                .TvCommand(TvCommand.Stop)
                .Start();

            Scenario.Create("TV Play")
                .SpeechTrigger("play", "Met play", "lecture", "Met la lecture")
                .TvCommand(TvCommand.Play)
                .Start();

            Scenario.Create("TV Play")
                .SpeechTrigger("éteint la télé")
                .TvCommand(TvCommand.Play)
                .Start();

            Scenario.Create("TV Augment volume")
                .SpeechTrigger("Monte le son", "Met le son", "Remet le son", "Augmente le volume", "Remet le volume")
                .TvSound(1)
                .Start();

            Scenario.Create("TV Baisse volume")
                .SpeechTrigger("Baisse le son", "Descent le son")
                .TvSound(-1)
                .Start();

            Scenario.Create("TV Mute")
                .SpeechTrigger("Coupe le son", "Coupe le volume")
                .TvMute()
                .Start();
        }
    }
}
