using Dommy.Business.Tools;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dommy.Web.Hubs
{
    public class Tile : Hub
    {
        public void UpdateTile(Dommy.Business.Tile tile)
        {
            Clients.Others.UpdateTile(tile);
        }
    }
}