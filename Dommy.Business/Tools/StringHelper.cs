//-----------------------------------------------------------------------
// <copyright file="StringHelper.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Format string with multiple format and random.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Initialize a random generator.
        /// </summary>
        private static Random r = new Random();

        /// <summary>
        /// Format string with data of properties.
        /// Example : "Hello {FirstName}" will be convert to by "Hello YourName" if the data contains a property FirstName.
        /// </summary>
        /// <param name="input">String format.</param>
        /// <param name="data">Object with properties used in the format.</param>
        /// <returns>Return string.</returns>
        public static string Format(string input, object data = null)
        {
            Dictionary<string, string> dico = new Dictionary<string, string>();

            if (data != null)
            {
                foreach (var p in data.GetType().GetProperties())
                {
                    dico.Add(p.Name, Convert.ToString(p.GetValue(data, new object[0]), CultureInfo.InvariantCulture));
                }
            }

            return Format(input, dico);
        }

        /// <summary>
        /// Format string with data of dictionary.
        /// Example : "Hello {FirstName}" will be convert to by "Hello YourName" if the data contains a key FirstName.
        /// </summary>
        /// <param name="input">String format.</param>
        /// <param name="data">Object with properties used in the format.</param>
        /// <returns>Return string.</returns>
        public static string Format(string input, Dictionary<string, string> data)
        {
            Regex reg = new Regex(@"\{([^\}]+)\}");

            return reg.Replace(input, new MatchEvaluator(
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
        /// <param name="inputs">String format. Use a format randomly.</param>
        /// <param name="data">Object with properties used in the format.</param>
        /// <returns>Return string.</returns>
        public static string Format(IList<string> inputs, object data = null)
        {
            Contract.Requires(inputs != null);

            int num = r.Next(inputs.Count);

            return StringHelper.Format(inputs[num], data);
        }
    }
}
