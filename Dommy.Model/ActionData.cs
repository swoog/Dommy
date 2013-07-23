using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Model
{
    //[KnownType(typeof(OnOffLightActionData))]
    [KnownType(typeof(TVData))]
    public class ActionData
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
