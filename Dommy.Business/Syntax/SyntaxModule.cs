﻿//-----------------------------------------------------------------------
// <copyright file="SyntaxModule.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Syntax
{
    using Ninject.Extensions.Conventions;
    using Ninject.Modules;

    /// <summary>
    /// Load syntax module.
    /// </summary>
    public class SyntaxModule : NinjectModule
    {
        /// <summary>
        /// Load the module.
        /// </summary>
        public override void Load()
        {
            this.Kernel.Bind(a => a.FromAssembliesMatching("*.dll").SelectAllClasses().InheritedFrom<IExtendSyntax>().BindDefaultInterface());
        }
    }
}
