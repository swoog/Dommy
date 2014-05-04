﻿
namespace Dommy.Business
{
    using System.Globalization;

    public class Tile
    {
        public string Title { get; set; }

        public System.Drawing.Color BackGroundColor { get; set; }

        public TileSize Size { get; set; }

        public string ColorCode
        {
            get
            {
                return "#" + this.BackGroundColor.R.ToString("X2", CultureInfo.InvariantCulture) + this.BackGroundColor.G.ToString("X2", CultureInfo.InvariantCulture) + this.BackGroundColor.B.ToString("X2", CultureInfo.InvariantCulture);
            }
        }

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
