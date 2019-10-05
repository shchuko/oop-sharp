namespace ShopCatalog.MariaDBDao.Exceptions
{
    public class DatabaseConnectErrorException : System.Exception
    {
        internal DatabaseConnectErrorException()
        {
            
        }

        internal DatabaseConnectErrorException(string what) : base(what)
        {
            
        }
        
    }
}