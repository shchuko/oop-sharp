using Bank.Clients;

namespace Bank.Accounts
{
    public abstract class AbstractAccount : IAccount
    {
        internal AbstractAccount(Client client)
        {
            _client = client;
        }
        
        public virtual bool TopUp(double subTotal)
        {
            if (subTotal < 0)
                return false;
            
            Total += subTotal;
            return true;
        }

        public virtual bool WriteOff(double subTotal)
        {
            if (subTotal < 0 || Total - subTotal < 0)
                return false;

            Total -= subTotal;
            return true;
        }

        public virtual bool TransferTo(IAccount abstractAccount, double subTotal)
        {
            return false;
        }

        public Client GetClient()
        {
            return _client;
        }

        public double GetTotal()
        {
            return Total;
        }

        public abstract double GetRate();
        public abstract string GetType();


        protected double Total;
        private readonly Client _client;
    }
}