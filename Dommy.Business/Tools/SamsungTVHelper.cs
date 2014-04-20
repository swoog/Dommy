using Dommy.Business.Configs;
using Ninject.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Dommy.Business.Tools
{
    public class SamsungTVHelper : ITVHelper
    {
        public class Config : IConfig
        {
            public string MyIP { get; set; }
            public string MyMac { get; set; }
            public string TvIP { get; set; }
            public void Create(Ninject.IKernel kernel)
            {
                kernel.Bind<ITVHelper>().To<SamsungTVHelper>()
                    .WithPropertyValue("MyIP", this.MyIP)
                    .WithPropertyValue("MyMac", this.MyMac)
                    .WithPropertyValue("TvIP", this.TvIP)
                    ;
            }
        }

        //Normal remote keys
        //KEY_0
        //KEY_1
        //KEY_2
        //KEY_3
        //KEY_4
        //KEY_5
        //KEY_6
        //KEY_7
        //KEY_8
        //KEY_9
        //KEY_UP
        //KEY_DOWN
        //KEY_LEFT
        //KEY_RIGHT
        //KEY_MENU
        //KEY_PRECH
        //KEY_GUIDE
        //KEY_INFO
        //KEY_RETURN
        //KEY_CH_LIST
        //KEY_EXIT
        //KEY_ENTER
        //KEY_SOURCE
        //KEY_AD
        //KEY_PLAY
        //KEY_PAUSE
        //KEY_MUTE
        //KEY_PICTURE_SIZE
        //KEY_VOLUP
        //KEY_VOLDOWN
        //KEY_TOOLS
        //KEY_POWEROFF
        //KEY_CHUP
        //KEY_CHDOWN
        //KEY_CONTENTS
        //KEY_W_LINK //Media P
        //KEY_RSS //Internet
        //KEY_MTS //Dual
        //KEY_CAPTION //Subt
        //KEY_REWIND
        //KEY_FF
        //KEY_REC
        //KEY_STOP

        //Bonus buttons not on the normal remote:
        //KEY_TV

        //Don't work/wrong codes:
        //KEY_CONTENT
        //KEY_INTERNET
        //KEY_PC
        //KEY_HDMI1
        //KEY_OFF
        //KEY_POWER
        //KEY_STANDBY
        //KEY_DUAL
        //KEY_SUBT
        //KEY_CHANUP
        //KEY_CHAN_UP
        //KEY_PROGUP
        //KEY_PROG_UP

        public string TvIP { get; set; }

        public string MyIP { get; set; }

        public string MyMac { get; set; }

        private ILogger logger;

        public SamsungTVHelper(ILogger logger)
        {
            this.logger = logger;
        }

        public void Canal(int canalNumber)
        {
            for (int i = Convert.ToString(canalNumber, CultureInfo.InvariantCulture).Length - 1; i >= 0; i--)
            {
                int mask = (int)Math.Pow(10, i);

                int val = canalNumber / mask;

                Command(String.Format(CultureInfo.InvariantCulture, "KEY_{0}", val));

                canalNumber -= val * mask;
            }
        }

        public void Command(TVCommand tvCommand)
        {
            Command(String.Format(CultureInfo.InvariantCulture, "KEY_{0}", tvCommand.ToString().ToUpper(CultureInfo.InvariantCulture)));
        }

        private void Command(string key)
        {
            Contract.Requires(key != null);

            this.logger.Info("TV {0}: {1}", this.TvIP, key);

            using (var ourMagicClient = new TcpClient())
            {

                try
                {
                    //Connect to the server - change this IP address to match your server's IP!
                    ourMagicClient.Connect(this.TvIP, 55000);
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

                var myip = this.MyIP;
                var mymac = this.MyMac;
                var remotename = "Dommy";
                var appstring = "iphone..iapp.samsung";
                var tvAppString = "iphone..iapp.samsung";

                //Use a NetworkStream object to send and/or receive some data
                NetworkStream ourStream = ourMagicClient.GetStream();

                var ipencoded = EncodeTo64(myip);
                var macencoded = EncodeTo64(mymac);
                var remotenameencoded = EncodeTo64(remotename);
                var appStringByte = System.Text.ASCIIEncoding.ASCII.GetBytes(appstring);
                var tvAppStringByte = System.Text.ASCIIEncoding.ASCII.GetBytes(tvAppString);

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

                //Preceding sections all first time only

                //if (isset($_REQUEST["key"])) {
                //Send remote key
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
                //} else if (isset($_REQUEST["text"])) {
                ////Send text, e.g. in YouTube app's search, N.B. NOT BBC iPlayer app.
                //$text = $_REQUEST["text"];
                //$messagepart3 = chr(0x01) . chr(0x00) . chr(strlen(base64_encode($text, ""))) . chr(0x00) . base64_encode($text, "");
                //$part3 = chr(0x01) . chr(strlen($appstring)) . chr(0x00) . $appstring . chr(strlen($messagepart3)) . chr(0x00) . $messagepart3;
                //socket_write($sock,$part3,strlen($part3));
                //echo $part3;
                //echo "\n";   
                //}
            }

            Thread.Sleep(TimeSpan.FromMilliseconds(500));
        }

        private class ByteWriter
        {
            private List<byte> bytes = new List<byte>();

            public void Append(byte b)
            {
                bytes.Add(b);
            }

            public void Append(byte[] b)
            {
                Contract.Requires(b != null);

                bytes.AddRange(b);
            }

            public void Append(ByteWriter writer)
            {
                Contract.Requires(writer != null);

                bytes.AddRange(writer.ToByteArray());
            }

            public byte[] ToByteArray()
            {
                return this.bytes.ToArray();
            }
        }

        private static byte[] Combine(params byte[][] arrays)
        {
            Contract.Requires(arrays != null);

            byte[] rv = new byte[arrays.Sum(a => a.Length)];
            int offset = 0;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        static public byte[] EncodeTo64(string toEncode)
        {
            Contract.Requires(toEncode != null);

            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return System.Text.ASCIIEncoding.ASCII.GetBytes(returnValue);
        }
    }
}
