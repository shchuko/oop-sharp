using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItmoNamespace
{
    interface ICloneable<T>
    {
        T GetClone();
    }
}
