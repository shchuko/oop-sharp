using System;

namespace ShopCatalog
{
    public class CSVFileConnectException : Exception
    {
        CSVFileConnectException()
        {
            
        }

        CSVFileConnectException(string what) : base(what)
        {
            
        }
    }
}