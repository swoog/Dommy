//-----------------------------------------------------------------------
// <copyright file="IKernelInfo.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Syntax
{
    using Ninject;

    /// <summary>
    /// Indicate implementation of the Ninject kernel property,
    /// </summary>
    public interface IKernelInfo
    {
        /// <summary>
        /// Gets Ninject kernel.
        /// </summary>
        IKernel Kernel { get; }
    }
}