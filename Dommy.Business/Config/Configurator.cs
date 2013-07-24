using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Dommy.Business.Config
{
    public class Configurator<T>
        where T : IConfig
    {
        public T Content { get; private set; }

        public Configurator(T content)
        {
            this.Content = content;
        }

        public Configurator<T> With<TResult>(Expression<Func<T, TResult>> property, TResult value)
        {
            var b = property.Body as MemberExpression;

            if (b == null)
            {
                throw new ArgumentException("the parameter must be an MemberExpression", "property");
            }

            var p = (PropertyInfo)(b.Member);

            p.SetValue(this.Content, value);

            return this;
        }
    }
}
