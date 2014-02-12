using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Business
{
    public class OrderAttribute : Attribute
    {
        public OrderAttribute(int order)
        {
            this.Order = order;
        }

        public int Order { get; private set; }
    }
}
