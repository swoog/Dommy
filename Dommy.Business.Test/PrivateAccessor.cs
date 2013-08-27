using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Test
{
    public class PrivateAccessor : DynamicObject
    {
        private object obj;

        public PrivateAccessor(object obj)
        {
            this.obj = obj;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var memberInfo = this.obj.GetType().GetMethod(binder.Name, BindingFlags.NonPublic | BindingFlags.Instance);

            result = memberInfo.Invoke(this.obj, args);
            return true;
        }
    }
}
