using ShopCatalog.Exceptions;

namespace ShopCatalog.MariaDBDao.Exceptions
{
    public class DatabaseConnectErrorException : DaoConnectException
    {
        internal DatabaseConnectErrorException()
        {
            
        }

        internal DatabaseConnectErrorException(string what) : base(what)
        {
            
        }
        
    }
}