
namespace Dommy.Business.Test
{
    using System.Dynamic;
    using System.Reflection;

    public class PrivateAccessor : DynamicObject
    {
        private object instance;

        public PrivateAccessor(object instance)
        {
            this.instance = instance;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var memberInfo = this.instance.GetType().GetMethod(binder.Name, BindingFlags.NonPublic | BindingFlags.Instance);

            result = memberInfo.Invoke(this.instance, args);
            return true;
        }
    }
}
