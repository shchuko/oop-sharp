using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItmoNamespace
{
    public class RationalFactory : IFactory<Rational>
    {
        public Rational createFromString(String s)
        {
            return new Rational(s);
        }
    }
}
