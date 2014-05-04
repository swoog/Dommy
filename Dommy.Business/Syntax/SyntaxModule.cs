
namespace Dommy.Business.Syntax
{
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;

    public class SyntaxModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IExtendSyntax>().BindDefaultInterface());
        }
    }
}
