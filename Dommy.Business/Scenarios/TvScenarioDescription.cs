//-----------------------------------------------------------------------
// <copyright file="TVScenarioDescription.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using Dommy.Business.Triggers;

    /// <summary>
    /// Description for TV scenarios.
    /// </summary>
    public class TVScenarioDescription : IScenarioDescription
    {
        /// <summary>
        /// TV helper used for scenarios.
        /// </summary>
        private ITvHelper tvHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="TVScenarioDescription"/> class.
        /// </summary>
        /// <param name="tvHelper">TV helper used for scenario.</param>
        public TVScenarioDescription(ITvHelper tvHelper)
        {
            this.tvHelper = tvHelper;
        }

        /// <summary>
        /// Create TV scenarios.
        /// </summary>
        public void Create()
        {
            for (int i = 0; i < Channel.ChannelsCount; i++)
            {
                var sentence = new[]
                {
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
                .TVCommand(TvCommand.Pause)
                .Start();

            Scenario.Create("TV Stop")
                .SpeechTrigger("Met stop", "stop")
                .TVCommand(TvCommand.Stop)
                .Start();

            Scenario.Create("TV Play")
                .SpeechTrigger("play", "Met play", "lecture", "Met la lecture")
                .TVCommand(TvCommand.Play)
                .Start();

            Scenario.Create("TV éteint")
                .SpeechTrigger("éteint la télé")
                .TVCommand(TvCommand.PowerOff)
                .Start();

            Scenario.Create("TV Augment volume")
                .SpeechTrigger("Monte le son", "Augmente le volume")
                .TVSound(5)
                .Start();

            Scenario.Create("TV Augment volume un peux")
                .SpeechTrigger("Monte un peux le son", "Met le son", "Remet le son", "Augmente un peux le volume", "Remet le volume")
                .TVSound(1)
                .Start();

            Scenario.Create("TV Baisse volume")
                .SpeechTrigger("Baisse le son", "Descent le son")
                .TVSound(-5)
                .Start();

            Scenario.Create("TV Baisse volume un peux")
                .SpeechTrigger("Baisse un peux le son", "Descent un peux le son")
                .TVSound(-1)
                .Start();

            Scenario.Create("TV Mute")
                .SpeechTrigger("Coupe le son", "Coupe le volume")
                .TVMute()
                .Start();
        }
    }
}
