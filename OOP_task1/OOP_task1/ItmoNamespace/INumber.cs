using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItmoNamespace
{
    interface INumber<T>
    {
        double GetDouble();
        int GetInt();
        bool IsGreaterThan(T objToCompare);
    }
}
