namespace Bank
{
    public class ClientBuilder
    {
        public ClientBuilder()
        {
            Reset();
        }

        public ClientBuilder Reset()
        {
            _client = new Client();
            return this;
        }

        public ClientBuilder SetName(string name)
        {
            _client.SetName(name);
            return this;
        }

        public ClientBuilder SetSurname(string surname)
        {
            _client.SetSurname(surname);
            return this;
        }

        public ClientBuilder SetAddress(string address)
        {
            _client.SetAddress(address);
            return this;
        }

        public ClientBuilder SetPassportId(string passportId)
        {
            _client.SetPassportId(passportId);
            return this;
        }

        public Client Build()
        {
            if (_client.Name != null && _client.Surname != null)
            {
                return _client;
            }    
            
            throw new ClassNotBuiltException();
        }
        
        private Client _client;
    }
}