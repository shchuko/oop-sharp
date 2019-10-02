using System;

namespace ShopCatalog
{
    public class ShopNotExistsException : Exception
    {
        ShopNotExistsException()
        {
            
        }
        
        ShopNotExistsException(string what) : base(what)
        {
            
        }
    }
}