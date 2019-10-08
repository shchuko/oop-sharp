
using ShopCatalog.Exceptions;

namespace ShopCatalog.CsvDao.Exceptions
{
    public class CsvFileConnectException : ServiceCreationException
    {
        internal CsvFileConnectException()
        {
            
        }

        internal CsvFileConnectException(string what) : base(what)
        {
            
        }
    }
}