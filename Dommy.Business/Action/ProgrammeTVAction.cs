//-----------------------------------------------------------------------
// <copyright file="ProgrammeTVAction.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Action
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel.Syndication;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using Dommy.Business.Result;
    using Dommy.Business.Tools;
    using Dommy.Model;

    /// <summary>
    /// TODO : Et sur la 6, et la 1, sur TF1, ...
    /// </summary>
    public class ProgrammeTVAction : BaseAction<ActionData>
    {
        private AsyncHelper async;

        public ProgrammeTVAction(AsyncHelper async)
        {
            this.async = async;
        }

        private class Programme
        {
            public string Channel { get; set; }

            public int Houre { get; set; }

            public int Minute { get; set; }

            public string Title { get; set; }
        }

        public override IList<string> Sentences
        {
            get
            {
                return new[]{
                    "Donne moi le programme télé",
                    "Qu'est-ce qu'il y a à la télé",
                    "Qu'elle est le programme télé",
                    "Qui y a t-il a la télé",
                };
            }
        }

        public override IResult RunAction(ISentence sentence)
        {
            return RunAction();
        }

        public override IResult RunAction()
        {
            return new PrecisionResult("Sur quel chaine ?", new[]{new PrecisionResult.SentenceAction()
            {
                 Action = InfoCannal,
                 Sentences = Channel.All().Select(c=>"La " + c.NumberToString).ToArray(),
                 UniqueKey = uniqueKey,
            }});
        }

        private string uniqueKey = Guid.NewGuid().ToString();

        private IResult InfoCannal(ISentence sentence)
        {
            var now = DateTime.Now;
            var time = now.Subtract(now.TimeOfDay).AddDays(1);

            var p = Cache.Get<List<Programme>>("ProgrammeTVAction", time.Subtract(now), () =>
            {
                return this.async.Wait(() =>
                {
                    SyndicationFeed feed = SyndicationFeed.Load(XmlReader.Create(String.Format("http://www.webnext.fr/epg_cache/programme-tv-xml_{0}-{1}-{2}.xml", now.Year, now.Month, now.Day)));

                    var programmes = new List<Programme>();

                    foreach (var item in feed.Items)
                    {
                        var elements = item.Title.Text.Split('|');

                        programmes.Add(new Programme
                        {
                            Channel = elements[0].Trim(),
                            Houre = Convert.ToInt32(elements[1].Trim().Split(':')[0]),
                            Minute = Convert.ToInt32(elements[1].Trim().Split(':')[1]),
                            Title = elements[2].Trim(),
                        });
                    }

                    return programmes;
                });
            });

            string channelName = String.Empty;

            if (sentence.Text.StartsWith("la", StringComparison.InvariantCultureIgnoreCase))
            {
                channelName = sentence.Text.Remove(0, 2);
            }
            else
            {
                channelName = sentence.Text;
            }

            var channel = Channel.FindChannel(channelName).FirstOrDefault();

            StringBuilder sb = new StringBuilder();

            foreach (var item in p.Where(prog => prog.Houre == 20 && prog.Minute > 30))
            {
                if (item.Channel.Equals(channel.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    sb.Append(item.Title);
                    sb.Append(" ");
                }
            }

            return new SayResult(sb.ToString());
        }
    }
}
