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
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime;

namespace Dommy.Console
{
    internal class Program
    {
#if DEBUG
        public const string ProcessX86 = @"..\..\..\Dommy.Console.x86\bin\Debug\Dommy.Console.x86.exe";
#else
        public const string ProcessX86 = "Dommy.Console.x86.exe";
#endif

        private static void Main(string[] args)
        {
            using (var p = Process.Start(ProcessX86))
            {
                Bootstrap.Run();
                p.Kill();
                }
            }
    }
}
