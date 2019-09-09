using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItmoNamespace
{
    public interface ISummable<T>
    {
        T Sum(T element);
    }
}
