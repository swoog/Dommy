using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Services
{
    [ServiceContract]
    public interface ITileManager
    {
        [OperationContract]
        IList<Tile> GetTiles();

        [OperationContract]
        Tile GetTile(int id);

        [OperationContract]
        void Start(int id);
    }
}
