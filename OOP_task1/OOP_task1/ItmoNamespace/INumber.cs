﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItmoNamespace
{
    public interface INumber
    {
        double GetDouble();
        int GetInt();
        bool IsGreaterThan(INumber objToCompare);
    }
}
