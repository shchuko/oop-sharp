using System;

namespace ShopCatalog
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