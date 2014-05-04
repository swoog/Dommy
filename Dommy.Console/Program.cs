
namespace Dommy.Console
{
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
            using (var p = Process.Start(ProcessX86))
            {
                Bootstrap.Run();
                p.Kill();
            }
        }
    }
}
