//-----------------------------------------------------------------------
// <copyright file="Tile.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System.Globalization;

    /// <summary>
    /// Data about a tile.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Gets or sets title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets background color.
        /// </summary>
        public System.Drawing.Color BackGroundColor { get; set; }

        /// <summary>
        /// Gets or sets size.
        /// </summary>
        public TileSize Size { get; set; }

        /// <summary>
        /// Gets color code.
        /// </summary>
        public string ColorCode
        {
            get
            {
                return "#" + this.BackGroundColor.R.ToString("X2", CultureInfo.InvariantCulture) + this.BackGroundColor.G.ToString("X2", CultureInfo.InvariantCulture) + this.BackGroundColor.B.ToString("X2", CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Gets width size.
        /// </summary>
        public int Width
        {
            get
            {
                switch (this.Size)
                {
                    case TileSize.Small: return 70;
                    case TileSize.MediumLine: return 150;
                    case TileSize.Medium: return 150;
                    case TileSize.Large: return 310;
                    default: return 70;
                }
            }
        }

        /// <summary>
        /// Gets height size.
        /// </summary>
        public int Height
        {
            get
            {
                switch (this.Size)
                {
                    case TileSize.Small: return 70;
                    case TileSize.MediumLine: return 70;
                    case TileSize.Medium: return 150;
                    case TileSize.Large: return 310;
                    default: return 70;
                }
            }
        }
    }
}
