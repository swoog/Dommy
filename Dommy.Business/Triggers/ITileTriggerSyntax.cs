﻿//-----------------------------------------------------------------------
// <copyright file="ITileTriggerSyntax.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Triggers
{
    using Dommy.Business.Syntax;
    using System.Drawing;

    /// <summary>
    /// Syntax of the tiles trigger.
    /// </summary>
    public interface ITileTriggerSyntax : IExtendSyntax
    {
        /// <summary>
        /// Create a tile on the web interface used to start scenario.
        /// </summary>
        /// <param name="title">Tile name.</param>
        /// <param name="backgroundColor">Background color of the tile.</param>
        /// <returns>Trigger scenario syntax.</returns>
        ITriggerScenarioSyntax TileTrigger(string title, Color backgroundColor);

        ITriggerScenarioSyntax TileTrigger(out Tile tile, string title, Color backgroundColor);

        ITriggerScenarioSyntax TileTrigger(string title, Color backgroundColor, string url);

        ITriggerScenarioSyntax TileTrigger(out Tile tile, string title, Color backgroundColor, string url);
    }
}