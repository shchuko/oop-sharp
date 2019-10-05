using System;

namespace ShopCatalog.Exceptions
{
    public class ServiceCreationException : Exception
    {
        internal ServiceCreationException()
        {
            
        }

        internal ServiceCreationException(string what) : base(what)
        {
            
        }
        
    }
}