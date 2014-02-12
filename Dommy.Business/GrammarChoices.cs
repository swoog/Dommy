using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business
{
    public class GrammarChoices
    {
        public GrammarChoices()
        {
            this.Elements = new List<string>();
        }

        public IList<string> Elements { get; private set; }

        internal void Add(string element)
        {
            this.Elements.Add(element);
        }
    }
}
