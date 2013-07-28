using Dommy.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dommy.Web.Models
{
    public class HomeIndexModel
    {
        public string Name { get; set; }

        public List<Tile> Tiles { get; set; }
    }
}