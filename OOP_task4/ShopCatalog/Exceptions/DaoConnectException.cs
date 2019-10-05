using System;

namespace ShopCatalog.Exceptions
{
    public class DaoConnectException : Exception
    {
        internal DaoConnectException() 
        {

        }

        internal DaoConnectException(string what) : base(what)
        {
            
        }
    }
}