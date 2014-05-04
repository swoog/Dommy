

namespace Dommy.Business.Tools
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.IO;
    using System.Net;
    using System.Text.RegularExpressions;
    using System.Xml;

    public class XmlReaderPatch : XmlTextReader
    {
        public static XmlTextReader GetPatchStream(string url)
        {
            Contract.Requires(!string.IsNullOrEmpty(url));

            WebRequest request = HttpWebRequest.Create(new Uri(url));

            var response = request.GetResponse();

            StreamReader reader = new StreamReader(response.GetResponseStream());

            string file = reader.ReadToEnd();

            file = file.Replace("Œ", "&#140;");

            file = Regex.Replace(file, "&([^;]{10})", m =>
            {
                return "&#38;" + m.Groups[1].Value;
            });

            var stringReader = new StringReader(file);

            return new XmlReaderPatch(stringReader);
        }

        private XmlReaderPatch(TextReader tr)
            : base(tr)
        {

        }

        private bool isLink = false;

        public override void ReadStartElement()
        {
            if (this.Name == "link")
            {
                isLink = true;
            }
            base.ReadStartElement();
        }

        public override void ReadEndElement()
        {
            if (this.Name == "link")
            {
                isLink = false;
            }

            base.ReadEndElement();
        }

        public override string ReadString()
        {
            if (this.isLink)
            {
                var v = base.ReadString();

                while (this.Name != "link")
                {
                    this.ReadEndElement();
                }

                return v;
            }

            string val = base.ReadString();

            if (String.IsNullOrEmpty(val) && this.Name == "pubDate")
            {
                return ToString(DateTime.Now);
            }

            DateTime d;

            var strinDate = Regex.Replace(val, @"\+([0-9][0-9])([0-9][0-9])", "+$1:$2");
            strinDate = Regex.Replace(strinDate, @"^[^,]+,\s+", "").Trim();
            strinDate = Regex.Replace(strinDate, @"^([0-9]{2}) ([^ ]+) ", m =>
            {
                for (int i = 1; i < 13; i++)
                {
                    var dt = new DateTime(2011, i, 1);
                    if (dt.ToString("MMMM", CultureInfo.GetCultureInfo("fr-FR").DateTimeFormat).StartsWith(m.Groups[2].Value, StringComparison.OrdinalIgnoreCase))
                    {
                        return m.Groups[1].Value + " " + dt.ToString("MMM", CultureInfo.InvariantCulture) + " ";
                    }
                }

                return m.Groups[0].Value;
            });

            strinDate = strinDate.Replace("GMT ", "");

            if (DateTime.TryParseExact(strinDate, "dd-MMM-yyyy HH:mm:ss zz", CultureInfo.CurrentCulture, DateTimeStyles.None, out d))
            {
                return ToString(d);
            }

            if (DateTime.TryParse(strinDate, CultureInfo.CurrentCulture, DateTimeStyles.None, out d))
            {
                return ToString(d);
            }

            if (DateTime.TryParse(val, out d))
            {
                return ToString(d);
            }

            return val;
        }

        private static string ToString(DateTime d)
        {
            var valDate = d.ToString("dd MMM yyyy HH:mm:ss zzzz", CultureInfo.InvariantCulture.DateTimeFormat);
            valDate = valDate.Remove(valDate.LastIndexOf(':'), 1);

            return valDate;
        }
    }

}
