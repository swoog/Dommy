//-----------------------------------------------------------------------
// <copyright file="ITvHelper.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Tools
{
    public interface ITVHelper
    {
        void Command(TVCommand tvCommand);

        void Canal(int canalNumber);
    }
}
