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
    using System.Text.RegularExpressions;

    /// <summary>
    /// Format string with multiple format and random.
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// Initialize a random generator.
        /// </summary>
        private static readonly Random RandomGenerator = new Random();

        /// <summary>
        /// Format string with data of properties.
        /// Example : "Hello {FirstName}" will be convert to by "Hello YourName" if the data contains a property FirstName.
        /// </summary>
        /// <param name="input">String format.</param>
        /// <param name="data">Object with properties used in the format.</param>
        /// <returns>Return string.</returns>
        public static string Format(string input, object data = null)
        {
            var dico = new Dictionary<string, string>();

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
        /// Format string with data of properties.
        /// Example : "Hello {FirstName}" will be convert to by "Hello YourName" if the data contains a property FirstName.
        /// </summary>
        /// <param name="inputs">String format. Use a format randomly.</param>
        /// <param name="data">Object with properties used in the format.</param>
        /// <returns>Return string.</returns>
        public static string Format(IList<string> inputs, object data = null)
        {
            Contract.Requires(inputs != null);

            int num = RandomGenerator.Next(inputs.Count);

            return Format(inputs[num], data);
        }

        /// <summary>
        /// Format string with data of dictionary.
        /// Example : "Hello {FirstName}" will be convert to by "Hello YourName" if the data contains a key FirstName.
        /// </summary>
        /// <param name="input">String format.</param>
        /// <param name="data">Object with properties used in the format.</param>
        /// <returns>Return string.</returns>
        private static string Format(string input, Dictionary<string, string> data)
        {
            Contract.Requires(input != null);

            var reg = new Regex(@"\{([^\}]+)\}");

            return reg.Replace(
                input,
                m =>
                    {
                        if (data.ContainsKey(m.Groups[1].Value))
                        {
                            return data[m.Groups[1].Value];
                        }

                        return "{" + m.Groups[1].Value + "}";
                    });
        }
    }
}
