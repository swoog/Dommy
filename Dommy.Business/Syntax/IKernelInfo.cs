using Ninject;

namespace Dommy.Business.Syntax
{
    public interface IKernelInfo
    {
        IKernel Kernel { get; }
    }
}