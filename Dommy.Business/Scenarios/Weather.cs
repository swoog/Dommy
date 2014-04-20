//-----------------------------------------------------------------------
// <copyright file="Weather.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Scenarios
{
    /// <summary>
    /// Class used to get weather details.
    /// </summary>
    internal class Weather
    {
        /// <summary>
        /// Gets or sets temperature.
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Gets or sets rain.
        /// </summary>
        public double Rain { get; set; }

        /// <summary>
        /// Gets or sets wind.
        /// </summary>
        public double Wind { get; set; }
    }
}
