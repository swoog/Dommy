//-----------------------------------------------------------------------
// <copyright file="EedomusAction.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Actions
{
    /// <summary>
    /// All eedomus action used in the API.
    /// </summary>
    public enum EedomusAction
    {
        /// <summary>
        /// Test authentication.
        /// </summary>
        AuthTest,

        /// <summary>
        /// Get value of eedomus element.
        /// </summary>
        PeriphValue,

        /// <summary>
        /// Get characteristic of eedomus element.
        /// </summary>
        PeriphCaract,

        /// <summary>
        /// Get history of eedomus element.
        /// </summary>
        PeriphHistory,
    }
}
