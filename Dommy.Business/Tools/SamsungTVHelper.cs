//-----------------------------------------------------------------------
// <copyright file="SamsungTVHelper.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Net.Sockets;
    using System.Threading;
    using Dommy.Business.Configs;

    using JetBrains.Annotations;

    using Ninject;
    using Ninject.Extensions.Logging;

    /// <summary>
    /// Samsung implement of TV helper.
    /// </summary>
    /// <remarks>
    /// Normal remote keys
    /// KEY_0
    /// KEY_1
    /// KEY_2
    /// KEY_3
    /// KEY_4
    /// KEY_5
    /// KEY_6
    /// KEY_7
    /// KEY_8
    /// KEY_9
    /// KEY_UP
    /// KEY_DOWN
    /// KEY_LEFT
    /// KEY_RIGHT
    /// KEY_MENU
    /// KEY_PRECH
    /// KEY_GUIDE
    /// KEY_INFO
    /// KEY_RETURN
    /// KEY_CH_LIST
    /// KEY_EXIT
    /// KEY_ENTER
    /// KEY_SOURCE
    /// KEY_AD
    /// KEY_PLAY
    /// KEY_PAUSE
    /// KEY_MUTE
    /// KEY_PICTURE_SIZE
    /// KEY_VOLUP
    /// KEY_VOLDOWN
    /// KEY_TOOLS
    /// KEY_POWEROFF
    /// KEY_CHUP
    /// KEY_CHDOWN
    /// KEY_CONTENTS
    /// KEY_W_LINK //Media P
    /// KEY_RSS //Internet
    /// KEY_MTS //Dual
    /// KEY_CAPTION //Subt
    /// KEY_REWIND
    /// KEY_FF
    /// KEY_REC
    /// KEY_STOP
    /// Bonus buttons not on the normal remote:
    /// KEY_TV
    /// Don't work/wrong codes:
    /// KEY_CONTENT
    /// KEY_INTERNET
    /// KEY_PC
    /// KEY_HDMI1
    /// KEY_OFF
    /// KEY_POWER
    /// KEY_STANDB
    /// KEY_DUAL
    /// KEY_SUBT
    /// KEY_CHANUP
    /// KEY_CHAN_UP
    /// KEY_PROGUP
    /// KEY_PROG_UP
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly",
        Justification = "Command of the TV.")]
    [UsedImplicitly]
    public class SamsungTvHelper : ITvHelper
    {
        /// <summary>
        /// Errors logger.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SamsungTvHelper"/> class.
        /// </summary>
        /// <param name="logger">Erros logger.</param>
        public SamsungTvHelper(ILogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Gets or sets the IP of user.
        /// </summary>
        [UsedImplicitly]
        public string MyIp { get; set; }

        /// <summary>
        /// Gets or sets the Mac of the user.
        /// </summary>
        /// <returns></returns>
        [UsedImplicitly]
        public string MyMac { get; set; }

        /// <summary>
        /// Gets or sets the IP of TV.
        /// </summary>
        [UsedImplicitly]
        public string TvIp { get; set; }

        /// <summary>
        /// Change canal of the TV.
        /// </summary>
        /// <param name="canalNumber">Cannal to change.</param>
        public void Canal(int canalNumber)
        {
            for (var i = Convert.ToString(canalNumber, CultureInfo.InvariantCulture).Length - 1; i >= 0; i--)
            {
                var mask = (int)Math.Pow(10, i);

                var val = canalNumber / mask;

                this.Command(string.Format(CultureInfo.InvariantCulture, "KEY_{0}", val));

                canalNumber -= val * mask;
            }
        }

        /// <summary>
        /// Execute a TV command.
        /// </summary>
        /// <param name="tvCommand">TV command.</param>
        public void Command(TvCommand tvCommand)
        {
            this.Command(string.Format(CultureInfo.InvariantCulture, "KEY_{0}", tvCommand.ToString().ToUpper(CultureInfo.InvariantCulture)));
        }

        /// <summary>
        /// Encode string to 64.
        /// </summary>
        /// <param name="toEncode">String to encode.</param>
        /// <returns>string in byte.</returns>
        private static byte[] EncodeTo64(string toEncode)
        {
            Contract.Requires(toEncode != null);

            var toEncodeAsBytes = System.Text.Encoding.ASCII.GetBytes(toEncode);

            string returnValue = Convert.ToBase64String(toEncodeAsBytes);

            return System.Text.Encoding.ASCII.GetBytes(returnValue);
        }

        /// <summary>
        /// Execute a command string.
        /// </summary>
        /// <param name="key">Key to execute.</param>
        private void Command(string key)
        {
            Contract.Requires(key != null);

            this.logger.Info("TV {0}: {1}", this.TvIp, key);

            using (var ourMagicClient = new TcpClient())
            {
                try
                {
                    // Connect to the server - change this IP address to match your server's IP!
                    ourMagicClient.Connect(this.TvIp, 55000);
                }
                catch (SocketException ex)
                {
                    if (ex.SocketErrorCode == SocketError.TimedOut)
                    {
                        this.logger.Info("TV not found");
                        return;
                    }

                    throw;
                }

                var myip = this.MyIp;
                var mymac = this.MyMac;
                const string Remotename = "Dommy";
                const string Appstring = "iphone..iapp.samsung";
                const string TvAppString = "iphone..iapp.samsung";

                // Use a NetworkStream object to send and/or receive some data
                var ourStream = ourMagicClient.GetStream();

                var ipencoded = EncodeTo64(myip);
                var macencoded = EncodeTo64(mymac);
                var remotenameencoded = EncodeTo64(Remotename);
                var appStringByte = System.Text.Encoding.ASCII.GetBytes(Appstring);
                var tvAppStringByte = System.Text.Encoding.ASCII.GetBytes(TvAppString);

                var messagePart1Writer = new ByteWriter();

                messagePart1Writer.Append(0x64);
                messagePart1Writer.Append(0x00);
                messagePart1Writer.Append(Convert.ToByte(ipencoded.Length));
                messagePart1Writer.Append(0x00);
                messagePart1Writer.Append(ipencoded);
                messagePart1Writer.Append(Convert.ToByte(macencoded.Length));
                messagePart1Writer.Append(0x00);
                messagePart1Writer.Append(macencoded);
                messagePart1Writer.Append(Convert.ToByte(remotenameencoded.Length));
                messagePart1Writer.Append(0x00);
                messagePart1Writer.Append(remotenameencoded);

                var messagePart1 = messagePart1Writer.ToByteArray();

                var part1Writer = new ByteWriter();
                part1Writer.Append(0x00);
                part1Writer.Append(Convert.ToByte(appStringByte.Length));
                part1Writer.Append(0x00);
                part1Writer.Append(appStringByte);
                part1Writer.Append(Convert.ToByte(messagePart1.Length));
                part1Writer.Append(0x00);
                part1Writer.Append(messagePart1);

                var part1 = part1Writer.ToByteArray();
                ourStream.Write(part1, 0, part1.Length);

                var messagePart2Writer = new ByteWriter();
                messagePart2Writer.Append(0xc8);
                messagePart2Writer.Append(0x00);
                var messagePart2 = messagePart2Writer.ToByteArray();

                var part2Writer = new ByteWriter();
                part2Writer.Append(0x00);
                part2Writer.Append(Convert.ToByte(appStringByte.Length));
                part2Writer.Append(0x00);
                part2Writer.Append(appStringByte);
                part2Writer.Append(Convert.ToByte(messagePart2.Length));
                part2Writer.Append(0x00);
                part2Writer.Append(messagePart2);
                var part2 = part2Writer.ToByteArray();
                ourStream.Write(part2, 0, part2.Length);

                // Preceding sections all first time only

                // if (isset($_REQUEST["key"])) {
                // Send remote key
                var messagePart3Writer = new ByteWriter();
                messagePart3Writer.Append(0x00);
                messagePart3Writer.Append(0x00);
                messagePart3Writer.Append(0x00);
                messagePart3Writer.Append(Convert.ToByte(EncodeTo64(key).Length));
                messagePart3Writer.Append(0x00);
                messagePart3Writer.Append(EncodeTo64(key));
                var messagepart3 = messagePart3Writer.ToByteArray();
                var part3Writer = new ByteWriter();
                part3Writer.Append(0x00);
                part3Writer.Append(Convert.ToByte(tvAppStringByte.Length));
                part3Writer.Append(0x00);
                part3Writer.Append(tvAppStringByte);
                part3Writer.Append(Convert.ToByte(messagepart3.Length));
                part3Writer.Append(0x00);
                part3Writer.Append(messagepart3);
                var part3 = part3Writer.ToByteArray();
                ourStream.Write(part3, 0, part3.Length);

                // } else if (isset($_REQUEST["text"])) {
                ////Send text, e.g. in YouTube app's search, N.B. NOT BBC iPlayer app.
                // $text = $_REQUEST["text"];
                // $messagepart3 = chr(0x01) . chr(0x00) . chr(strlen(base64_encode($text, ""))) . chr(0x00) . base64_encode($text, "");
                // $part3 = chr(0x01) . chr(strlen($appstring)) . chr(0x00) . $appstring . chr(strlen($messagepart3)) . chr(0x00) . $messagepart3;
                // socket_write($sock,$part3,strlen($part3));
                // echo $part3;
                // echo "\n";   
                // }
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(500));
        }

        /// <summary>
        /// Samsung TV configuration.
        /// </summary>
        public class Config : IConfig
        {
            /// <summary>
            /// Gets or sets IP of the user.
            /// </summary>
            [UsedImplicitly]
            public string MyIp { get; set; }

            /// <summary>
            /// Gets or sets Mac of the user.
            /// </summary>
            [UsedImplicitly]
            public string MyMac { get; set; }

            /// <summary>
            /// Gets or sets IP of the TV.
            /// </summary>
            [UsedImplicitly]
            public string TvIp { get; set; }

            /// <summary>
            /// Create the Samsung TV helper.
            /// </summary>
            /// <param name="kernel">Ninject kernel.</param>
            public void Create(IKernel kernel)
            {
                kernel.Bind<ITvHelper>().To<SamsungTvHelper>()
                    .WithPropertyValue("MyIp", this.MyIp)
                    .WithPropertyValue("MyMac", this.MyMac)
                    .WithPropertyValue("TvIp", this.TvIp);
            }
        }

        /// <summary>
        /// Byte writer class.
        /// </summary>
        private class ByteWriter
        {
            /// <summary>
            /// List of bytes.
            /// </summary>
            private readonly List<byte> bytes = new List<byte>();

            /// <summary>
            /// Append byte.
            /// </summary>
            /// <param name="b">A byte.</param>
            public void Append(byte b)
            {
                this.bytes.Add(b);
            }

            /// <summary>
            /// Append a new byte array.
            /// </summary>
            /// <param name="b">Byte array.</param>
            public void Append(IEnumerable<byte> b)
            {
                Contract.Requires(b != null);

                this.bytes.AddRange(b);
            }

            /// <summary>
            /// Convert byte to array.
            /// </summary>
            /// <returns>Byte array.</returns>
            public byte[] ToByteArray()
            {
                return this.bytes.ToArray();
            }
        }
    }
}
