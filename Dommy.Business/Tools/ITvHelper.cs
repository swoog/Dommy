//-----------------------------------------------------------------------
// <copyright file="ITvHelper.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    /// <summary>
    /// TV helper interface.
    /// </summary>
    public interface ITvHelper
    {
        /// <summary>
        /// Execute a command to TV.
        /// </summary>
        /// <param name="tvCommand">Command to execute.</param>
        void Command(TVCommand tvCommand);

        /// <summary>
        /// Change canal.
        /// </summary>
        /// <param name="canalNumber">Canal number.</param>
        void Canal(int canalNumber);
    }
}
