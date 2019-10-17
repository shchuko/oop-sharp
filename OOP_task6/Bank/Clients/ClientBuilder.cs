using Bank.Clients.Exceptions;

namespace Bank.Clients
{
    public class ClientBuilder
    {
        public ClientBuilder()
        {
            Reset();
        }

        public ClientBuilder Reset()
        {
            _name = null;
            _address = null;
            _surname = null;
            _passportId = null;
            return this;
        }

        public ClientBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public ClientBuilder SetSurname(string surname)
        {
            _surname = surname;
            return this;
        }

        public ClientBuilder SetAddress(string address)
        {
            _address = address;
            return this;
        }

        public ClientBuilder SetPassportId(string passportId)
        {
            _passportId = passportId;
            return this;
        }

        public Client Build()
        {
            if (_name != null && _surname != null)
            {
                Client client = new Client();
                client.SetName(_name);
                client.SetSurname(_surname);
                client.SetPassportId(_passportId);
                client.SetAddress(_address);
                client.UpdVerificationStatus();
                return client;
            }    
            
            throw new ClassNotBuiltException();
        }
        
        private string _name;
        private string _surname;
        private string _address;
        private string _passportId;
    }
}