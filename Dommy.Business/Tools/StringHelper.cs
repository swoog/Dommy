//-----------------------------------------------------------------------
// <copyright file="StringHelper.cs" company="Microsoft">
//     Copyright (c) agaltier, Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Format string with multiple format and random.
    /// </summary>
    public class StringHelper
    {
        public static string Format(string format, object obj = null)
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();

            if (obj != null)
            {
                foreach (var p in obj.GetType().GetProperties())
                {
                    dico.Add(p.Name, Convert.ToString(p.GetValue(obj, new object[0])));
                }
            }

            return Format(format, dico);
        }

        public static string Format(string format, Dictionary<string, string> values)
        {
            Regex reg = new Regex(@"\{([^\}]+)\}");

            return reg.Replace(format, new MatchEvaluator(m =>
            {
                if (values.ContainsKey(m.Groups[1].Value))
                {
                    return values[m.Groups[1].Value];
                }
                else
                {
                    return "{" + m.Groups[1].Value + "}";
                }
            }));
        }

        private static Random r = new Random();

        public static string Format(IList<string> formats, object obj = null)
        {
            int num = r.Next(formats.Count);

            return StringHelper.Format(formats[num], obj);
        }
    }
}
