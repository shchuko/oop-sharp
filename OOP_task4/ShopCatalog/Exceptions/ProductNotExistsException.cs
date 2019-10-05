using System;

namespace ShopCatalog.Exceptions
{
    public class ProductNotExistsException : Exception
    {
        internal ProductNotExistsException()
        {
            
        }

        internal ProductNotExistsException(string what) : base(what)
        {
            
        }
    }
}