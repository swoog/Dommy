//-----------------------------------------------------------------------
// <copyright file="OrderAttribute.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Attribute indicate the order of listener.
    /// </summary>
    public class OrderAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OrderAttribute"/> class.
        /// </summary>
        /// <param name="order">Order value.</param>
        public OrderAttribute(int order)
        {
            this.Order = order;
        }

        /// <summary>
        /// Gets order value.
        /// </summary>
        /// <returns></returns>
        public int Order { get; private set; }
    }
}
