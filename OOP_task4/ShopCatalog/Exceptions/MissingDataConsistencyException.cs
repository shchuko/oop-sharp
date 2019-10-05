using System;

namespace ShopCatalog.Exceptions
{
    public class MissingDataConsistencyException : Exception
    {
        internal MissingDataConsistencyException()
        {
            
        }

        internal MissingDataConsistencyException(string what) : base(what)
        {
            
        }
    }
}