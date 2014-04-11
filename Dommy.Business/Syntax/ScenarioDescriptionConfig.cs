//-----------------------------------------------------------------------
// <copyright file="ScenarioDescriptionConfig.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Syntax
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Linq.Expressions;
    using Ninject.Syntax;

    /// <summary>
    /// Description of the configuration of a scenario.
    /// </summary>
    /// <typeparam name="T">Description scenario.</typeparam>
    public class ScenarioDescriptionConfig<T>
    {
        /// <summary>
        /// Instance of the configurator.
        /// </summary>
        private IBindingWithSyntax<T> configurator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScenarioDescriptionConfig{T}"/> class.
        /// </summary>
        /// <param name="configurator">Instance of the configurator.</param>
        public ScenarioDescriptionConfig(IBindingWithSyntax<T> configurator)
        {
            this.configurator = configurator;
        }

        /// <summary>
        /// Configure a property of the scenario.
        /// </summary>
        /// <typeparam name="TReturn">Type of the config.</typeparam>
        /// <param name="property">Expression representing a property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Return this instance of scenario description config.</returns>
        public ScenarioDescriptionConfig<T> With<TReturn>(Expression<Func<T, TReturn>> property, TReturn value)
        {
            Contract.Requires(property != null);

            var b = property.Body as MemberExpression;

            if (b == null)
            {
                throw new ArgumentException("the parameter must be an MemberExpression", "property");
            }

            this.configurator = this.configurator.WithPropertyValue(b.Member.Name, value);

            return this;
        }
    }
}
