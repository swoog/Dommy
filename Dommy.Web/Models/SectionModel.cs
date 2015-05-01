using Dommy.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dommy.Web.Models
{
    public class SectionModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Tile> Tiles { get; set; }

        public string Color { get; set; }
    }
}