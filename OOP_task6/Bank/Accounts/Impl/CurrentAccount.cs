using Bank.Clients;

namespace Bank.Accounts.Impl
{
    public class CurrentAccount : AbstractAccount
    {
        internal CurrentAccount(Client client, double rate) : base(client)
        {
            _rate = rate;
        }

        private double _rate;
        public override double GetRate()
        {
            return _rate;
        }

        public override string GetAccountType()
        {
            return "Current Account";
        }
    }
    
}