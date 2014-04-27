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
using System.IO;
using System.Linq;
using System.Runtime;

namespace Dommy.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Bootstrap.Run();
        }
    }
}
