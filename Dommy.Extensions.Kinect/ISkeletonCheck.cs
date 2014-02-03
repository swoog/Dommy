using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dommy.Extensions.Kinect
{
    public interface ISkeletonCheck
    {
        bool Check(Microsoft.Kinect.Skeleton skeleton);
    }
}
