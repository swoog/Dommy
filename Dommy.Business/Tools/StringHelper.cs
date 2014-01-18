//-----------------------------------------------------------------------
// <copyright file="StringHelper.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Format string with multiple format and random.
    /// </summary>
    public class StringHelper
    {
        /// <summary>
        /// Initialize a random generator.
        /// </summary>
        private static Random r = new Random();

        /// <summary>
        /// Format string with data of properties.
        /// Example : "Hello {FirstName}" will be convert to by "Hello YourName" if the data contains a property FirstName.
        /// </summary>
        /// <param name="format">String format.</param>
        /// <param name="data">Object with properties used in the format.</param>
        /// <returns>Return string.</returns>
        public static string Format(string format, object data = null)
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();

            if (data != null)
            {
                foreach (var p in data.GetType().GetProperties())
                {
                    dico.Add(p.Name, Convert.ToString(p.GetValue(data, new object[0])));
                }
            }

            return Format(format, dico);
        }

        /// <summary>
        /// Format string with data of dictionary.
        /// Example : "Hello {FirstName}" will be convert to by "Hello YourName" if the data contains a key FirstName.
        /// </summary>
        /// <param name="format">String format.</param>
        /// <param name="data">Object with properties used in the format.</param>
        /// <returns>Return string.</returns>
        public static string Format(string format, Dictionary<string, string> data)
        {
            Regex reg = new Regex(@"\{([^\}]+)\}");

            return reg.Replace(format, new MatchEvaluator(
                m =>
                {
                    if (data.ContainsKey(m.Groups[1].Value))
                    {
                        return data[m.Groups[1].Value];
                    }
                    else
                    {
                        return "{" + m.Groups[1].Value + "}";
                    }
                }));
        }

        /// <summary>
        /// Format string with data of properties.
        /// Example : "Hello {FirstName}" will be convert to by "Hello YourName" if the data contains a property FirstName.
        /// </summary>
        /// <param name="formats">String format. Used randomly a format.</param>
        /// <param name="data">Object with properties used in the format.</param>
        /// <returns>Return string.</returns>
        public static string Format(IList<string> formats, object data = null)
        {
            int num = r.Next(formats.Count);

            return StringHelper.Format(formats[num], data);
        }
    }
}
