
namespace Dommy.Web.Models
{
    using System.Collections.Generic;
    using Dommy.Business;

    public class HomeIndexModel
    {
        public string Name { get; set; }

        public IList<Tile> Tiles { get; set; }
    }
}