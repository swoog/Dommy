using Ninject.Syntax;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Dommy.Business.Syntax
{
    public class ScenarioDescriptionConfig<T>
    {
        private IBindingWithSyntax<T> c;

        public ScenarioDescriptionConfig(IBindingWithSyntax<T> c)
        {
            this.c = c;
        }

        public ScenarioDescriptionConfig<T> With<TReturn>(Expression<Func<T, TReturn>> property, TReturn value)
        {
            Contract.Requires(property != null);

            var b = property.Body as MemberExpression;

            if (b == null)
            {
                throw new ArgumentException("the parameter must be an MemberExpression", "property");
            }

            this.c = this.c.WithPropertyValue(b.Member.Name, value);

            return this;
        }
    }
}
