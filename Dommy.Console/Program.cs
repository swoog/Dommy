//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Console
{
    using System;
    using System.Diagnostics;
    using Dommy.Business;

    public class Program
    {
#if DEBUG
        public const string ProcessX86 = @"..\..\..\Dommy.Console.x86\bin\Debug\Dommy.Console.x86.exe";
#else
        public const string ProcessX86 = "Dommy.Console.x86.exe";
#endif

        static void Main(string[] args)
        {
            var startInfo = new ProcessStartInfo(ProcessX86);
            startInfo.CreateNoWindow = true;

            using (var p = Process.Start(startInfo))
            {
                p.Exited += x86Exited;
                Bootstrap.Run();
                try
                {
                    p.Exited -= x86Exited;
                    p.Kill();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private static void x86Exited(object sender, EventArgs e)
        {
            // TODO : Log error
            // TODO : Restart
        }
    }
}
