namespace Bank
{
    public class Client
    {
        public string Name => _name;
        public string Surname => _surname;
        public string Address => _address;
        public string PassportId => _passportId;
        
        private string _name;
        private string _surname;
        private string _address;
        private string _passportId;

        internal Client()
        {
            
        }
        
        internal void SetName(string name)
        {
            _name = name;
        }

        internal void SetSurname(string surname)
        {
            _surname = surname;
        }

        internal void SetAddress(string address)
        {
            _address = address;
        }

        internal void SetPassportId(string passportId)
        {
            _passportId = passportId;
        }
    }
}