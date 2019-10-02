using System;

namespace ShopCatalog
{
    public class CSVFileConnectException : Exception
    {
        internal CSVFileConnectException()
        {
            
        }

        internal CSVFileConnectException(string what) : base(what)
        {
            
        }
    }
}