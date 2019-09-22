namespace ParserINI.Exceptions
{
    public class KeyNotFoundException : System.Exception
    {
        public KeyNotFoundException()
        {
            _keyNameName = "";
        }
        
        public KeyNotFoundException(string keyName) : base(keyName)
        {
            _keyNameName = keyName;
        }

        public string GetKeyName()
        {
            return _keyNameName;
        }

        private string _keyNameName;
    }
}