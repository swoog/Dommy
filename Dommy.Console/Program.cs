//-----------------------------------------------------------------------
// <copyright file="vctmp4388_621514.Program.f2225527merge.cs" company="">
//     
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
    internal class Program
    {
#if DEBUG
        public const string ProcessX86 = @"..\..\..\Dommy.Console.x86\bin\Debug\Dommy.Console.x86.exe";
#else
        public const string ProcessX86 = "Dommy.Console.x86.exe";
#endif

        private static void Main(string[] args)
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
