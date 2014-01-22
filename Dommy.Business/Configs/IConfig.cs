//-----------------------------------------------------------------------
// <copyright file="IConfig.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Business.Configs
{
    using Ninject;

    /// <summary>
    /// Interface config.
    /// </summary>
    public interface IConfig
    {
        /// <summary>
        /// When inherited create configuration.
        /// </summary>
        /// <param name="kernel">Ninject kernel to create config.</param>
        void Create(IKernel kernel);
    }
}
