namespace Bank
{
    public class CurrentAbstractAccount : AbstractAccount
    {
        internal CurrentAbstractAccount(Client client, double rate) : base(client)
        {
            _rate = rate;
        }

        private double _rate;
    }
    
}