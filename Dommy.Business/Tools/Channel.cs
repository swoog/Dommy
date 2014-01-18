using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Tools
{
    public class Channel
    {
        private static string[] numbers = new[]{
            "une",
            "deux",
            "trois",
            "quatre",
            "cinq",
            "six",
            "sept",
            "huit",
            "neuf",
            "dix",
            "onze",
            "douze",
            "treize",
            "quatorze",
            "quinze",
            "seize",
            "dix sept",
            "dix huit",
            "dix neuf",
            "vingt",
            "vingt-un",
            "vingt-deux",
            "vingt-trois",
            "vingt-quatre",
            "vingt-cinq",
            "vingt-six",
            "vingt-sept",
            "vingt-huit",
            "vingt-neuf",
            "trente",
        };

        private static string[] names = new[]{
            "tf1",
            "france2",
            "france3",
            "quatre",
            "cinq",
            "m6",
            "sept",
            "huit",
            "neuf",
            "dix",
            "onze",
            "douze",
            "treize",
            "france4",
            "quinze",
            "seize",
            "dix sept",
            "dix huit",
            "dix neuf",
            "vingt",
            "vingt-un",
            "vingt-deux",
            "vingt-trois",
            "vingt-quatre",
            "vingt-cinq",
            "vingt-six",
            "vingt-sept",
            "vingt-huit",
            "vingt-neuf",
            "trente",
        };

        private static Channel[] channels;

        public static int ChannelsCount
        {
            get
            {
                InitChannels();

                return channels.Length;
            }
        }

        private static void InitChannels()
        {
            if (channels == null)
            {
                channels = new Channel[names.Length];

                for (int i = 0; i < names.Length; i++)
                {
                    channels[i] = new Channel()
                    {
                        NumberToString = numbers[i],
                        Number = i + 1,
                        Name = names[i],
                    };
                }
            }
        }

        public static Channel GetChannel(int number)
        {
            InitChannels();

            return channels[number];
        }

        public static IEnumerable<Channel> FindChannel(string text)
        {
            InitChannels();

            var q = from c in channels
                    where c.Name.Equals(text.Replace(" ", ""), StringComparison.OrdinalIgnoreCase)
                    || c.NumberToString.Equals(text.Replace(" ", ""), StringComparison.OrdinalIgnoreCase)
                    select c;

            return q;
        }

        public static IEnumerable<Channel> All()
        {
            InitChannels();

            return channels;
        }

        public string Name { get; private set; }

        public string NumberToString { get; set; }

        public int Number { get; set; }
    }
}
