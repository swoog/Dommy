using Ninject.Modules;
using Ninject.Extensions.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dommy.Business.Syntax
{
    public class SyntaxModule : NinjectModule
    {
        public override void Load()
        {
            this.Kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IExtendSyntax>().BindDefaultInterface());
        }
    }
}
