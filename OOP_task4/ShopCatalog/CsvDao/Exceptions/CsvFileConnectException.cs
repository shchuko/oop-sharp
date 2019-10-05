using ShopCatalog.Exceptions;

namespace ShopCatalog.CsvDao.Exceptions
{
    public class CsvFileConnectException : DaoConnectException
    {
        internal CsvFileConnectException()
        {
            
        }

        internal CsvFileConnectException(string what) : base(what)
        {
            
        }
    }
}