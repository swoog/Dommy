﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Dommy.Business
{
    public class Tile
    {
        public string SectionName { get; set; }

        public string Title { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TileColor Color { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
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

        public void Setembedded(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                this.View = reader.ReadToEnd();
            }
        }
    }
}
