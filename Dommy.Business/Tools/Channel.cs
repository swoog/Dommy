//-----------------------------------------------------------------------
// <copyright file="Channel.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;

    /// <summary>
    /// TV channel name and value.
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// Text numbers.
        /// </summary>
        private static readonly string[] Numbers =
        {
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
            "trente"
        };

        /// <summary>
        /// Name of each channel.
        /// </summary>
        private static readonly string[] Names =
        {
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
            "trente"
        };

        /// <summary>
        /// List of channels data.
        /// </summary>
        private static Channel[] channels;

        /// <summary>
        /// Gets the number of channel.
        /// </summary>
        /// <returns>Number of channel.</returns>
        public static int ChannelsCount
        {
            get
            {
                InitChannels();

                return channels.Length;
            }
        }

        /// <summary>
        /// Gets the text of a number.
        /// </summary>
        /// <returns></returns>
        public string NumberToString { get; private set; }

        /// <summary>
        /// Gets the number of the channel.
        /// </summary>
        /// <returns></returns>
        public int Number { get; private set; }

        /// <summary>
        /// Get channel from a number.
        /// </summary>
        /// <param name="number">Number of the channel.</param>
        /// <returns>Channel instance class.</returns>
        public static Channel GetChannel(int number)
        {
            Contract.Requires(0 <= number);

            InitChannels();

            return channels[number];
        }

        /// <summary>
        /// Get all channels.
        /// </summary>
        /// <returns>All channels</returns>
        public static IEnumerable<Channel> All()
        {
            InitChannels();

            return channels;
        }

        /// <summary>
        /// Initialize channels.
        /// </summary>
        private static void InitChannels()
        {
            if (channels == null)
            {
                channels = new Channel[Names.Length];

                for (var i = 0; i < Names.Length; i++)
                {
                    channels[i] = new Channel
                    {
                        NumberToString = Numbers[i],
                        Number = i + 1,
                    };
                }
            }
        }
    }
}
