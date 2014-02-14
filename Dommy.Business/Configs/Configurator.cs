//-----------------------------------------------------------------------
// <copyright file="Configurator.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Configs
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Configuration class.
    /// </summary>
    /// <typeparam name="T">Type of class used for configuration. Type must inherit from <see cref="IConfig"/>.</typeparam>
    public class Configurator<T>
        where T : IConfig
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Configurator{T}"/> class.
        /// </summary>
        /// <param name="content">Instance of configuration class.</param>
        public Configurator(T content)
        {
            this.Content = content;
        }

        /// <summary>
        /// Gets instance of configuration class.
        /// </summary>
        public T Content { get; private set; }

        /// <summary>
        /// Set property of class to a value.
        /// </summary>
        /// <typeparam name="TResult">Type of the property.</typeparam>
        /// <param name="property">Lambda expression of property member.</param>
        /// <param name="value">Value to set to the property.</param>
        /// <returns>Configuration class.</returns>
        public Configurator<T> With<TResult>(Expression<Func<T, TResult>> property, TResult value)
        {
            Contract.Requires(property != null);

            var b = property.Body as MemberExpression;

            if (b == null)
            {
                throw new ArgumentException("the parameter must be an MemberExpression", "property");
            }

            var p = (PropertyInfo)b.Member;

            p.SetValue(this.Content, value);

            return this;
        }
    }
}
