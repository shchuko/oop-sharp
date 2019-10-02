using System;

namespace ShopCatalog
{
    public class ProductNotExistsException : Exception
    {
        ProductNotExistsException()
        {
            
        }

        ProductNotExistsException(string what) : base(what)
        {
            
        }
    }
}