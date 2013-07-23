//-----------------------------------------------------------------------
// <copyright file="SayResult.cs" company="Microsoft">
//     Copyright (c) agaltier, Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace Dommy.Business.Result
{
    using System;
    using System.Linq;

    /// <summary>
    /// Say text for result.
    /// </summary>
    public class SayResult : IResult
    {
        public string Speech { get; private set; }

        public SayResult(string speech)
        {
            this.Speech = speech;
        }
    }
}
