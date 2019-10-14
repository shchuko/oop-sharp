using System;

namespace Bank
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
            if (!GetClient().Equals(abstractAccount.GetClient()))
                return false;
            if (!WriteOff(subTotal))
                return false;
            if (!abstractAccount.TopUp(subTotal))
            {
                TopUp(subTotal);
                return false;
            }

            return true;
        }

        public Client GetClient()
        {
            return _client;
        }


        protected double Total;
        private readonly Client _client;
    }
}