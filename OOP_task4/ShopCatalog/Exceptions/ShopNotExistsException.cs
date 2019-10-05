using System;

namespace ShopCatalog.Exceptions
{
    public class ShopNotExistsException : Exception
    {
        internal ShopNotExistsException()
        {
            
        }
        
        internal ShopNotExistsException(string what) : base(what)
        {
            
        }
    }
}