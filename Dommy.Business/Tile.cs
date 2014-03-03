using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Dommy.Business
{
    public class Tile
    {
        public string SectionName { get; set; }

        public string Title { get; set; }

        public TileColor Color { get; set; }

        public TileSize Size { get; set; }

        public object Data { get; set; }

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

        public int Id { get; set; }

        public string Url { get; set; }

        public string View { get; set; }
    }
}