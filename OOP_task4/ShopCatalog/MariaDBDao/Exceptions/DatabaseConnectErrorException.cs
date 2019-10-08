using ShopCatalog.Exceptions;

namespace ShopCatalog.MariaDBDao.Exceptions
{
    public class DatabaseConnectErrorException : ServiceCreationException
    {
        internal DatabaseConnectErrorException()
        {
            
        }

        internal DatabaseConnectErrorException(string what) : base(what)
        {
            
        }
        
    }
}