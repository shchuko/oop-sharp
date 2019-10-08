using System;

namespace ShopCatalog.Exceptions
{
    public class DataReadingErrorException : Exception
    {
        internal DataReadingErrorException()
        {
            
        }

        internal DataReadingErrorException(string what) : base(what)
        {
            
        }
    }
}