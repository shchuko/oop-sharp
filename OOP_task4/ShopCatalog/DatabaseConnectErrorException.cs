using System;

namespace ShopCatalog
{
    public class DatabaseConnectErrorException : Exception
    {
        internal DatabaseConnectErrorException()
        {
            
        }

        internal DatabaseConnectErrorException(string what) : base(what)
        {
            
        }
        
    }
}