using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Syntax
{
    /// <summary>
    /// 
    /// </summary>
    public class ElementName
    {
        public ElementName(string name, string prefixName, Gender gender)
        {
            this.Name = name;
            this.PrefixName = prefixName;
            this.Gender = gender;
        }

        public string Name { get; private set; }

        /// <summary>
        /// Exemple : de la salle, du salon,...
        /// </summary>
        public string PrefixName { get; private set; }

        public Gender Gender { get; private set; }

        public override string ToString()
        {
            return this.Name;
        }
    }


}
