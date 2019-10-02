using System;

namespace ShopCatalog
{
    public class DatabaseConnectErrorException : Exception
    {
        DatabaseConnectErrorException()
        {
            
        }

        DatabaseConnectErrorException(string what) : base(what)
        {
            
        }
        
    }
}