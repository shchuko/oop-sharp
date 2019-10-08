using ShopCatalog.Exceptions;

namespace ShopCatalog.MariaDBDao.Exceptions
{
    public class DatabaseDataReadingException : DataReadingErrorException
    {
        internal DatabaseDataReadingException() {}

        internal DatabaseDataReadingException(string what) : base(what)
        {
            
        }
    }
}