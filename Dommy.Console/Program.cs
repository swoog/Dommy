//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="TrollCorp">
//     Copyright (c) agaltier, TrollCorp. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Dommy.Console
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime;
    using Dommy.Business;
    using Dommy.Business.Configs;
    using Dommy.Business.Scenarios;
    using Dommy.Business.Scripts;
    using Dommy.Business.Services;
    using Dommy.Business.Syntax;
    using Dommy.Business.Tools;
    using Dommy.Business.WebHost;
    using Dommy.Extensions.Kinect;
    using Ninject;
    using Ninject.Extensions.Conventions;
    public class Program
    {
#if DEBUG
        public const string ProcessX86 = @"..\..\..\Dommy.Console.x86\bin\Debug\Dommy.Console.x86.exe";
#else
        public const string ProcessX86 = "Dommy.Console.x86.exe";
#endif

        private static Process x86Process;

        static void Main(string[] args)
        {
            var startInfo = new ProcessStartInfo(ProcessX86);
            startInfo.CreateNoWindow = true;
            startInfo.ErrorDialog = false;

            x86Process = Process.Start(startInfo);
            if (x86Process != null)
            {
                x86Process.EnableRaisingEvents = true;
                x86Process.Exited += X86Exited;
                Bootstrap.Run();
                try
                {
                    x86Process.Exited -= X86Exited;
                    x86Process.Kill();
                }
                catch (Exception)
                {
                    throw;
                }

                x86Process.Dispose();
            }
        }

        private static void X86Exited(object sender, EventArgs e)
        {
            x86Process.Exited -= X86Exited;
            // Restart
            var startInfo = new ProcessStartInfo(ProcessX86);
            startInfo.CreateNoWindow = true;
            x86Process = Process.Start(startInfo);

            if (x86Process != null)
            {
                x86Process.EnableRaisingEvents = true;
                x86Process.Exited += X86Exited;
            }
        }
    }
}
