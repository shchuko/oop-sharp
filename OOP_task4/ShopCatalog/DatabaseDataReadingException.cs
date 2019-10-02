namespace ShopCatalog
{
    public class DatabaseDataReadingException : System.Exception
    {
        internal DatabaseDataReadingException() {}

        internal DatabaseDataReadingException(string what) : base(what)
        {
            
        }
    }
}