using Bank.Clients;

namespace Bank.Accounts.Wrappers
{
    public abstract class AccountWrapper : IAccount
    {
        internal AccountWrapper(IAccount source) 
        {
            _wrappee = source;
        }

        public virtual bool TransferTo(IAccount abstractAccount, double subTotal)
        {
            return _wrappee.TransferTo(abstractAccount, subTotal);
        }

        public virtual Client GetClient()
        {
            return _wrappee.GetClient();
        }

        public double GetTotal()
        {
            return _wrappee.GetTotal();
        }

        public double GetRate()
        {
            return _wrappee.GetRate();
        }

        public string GetType()
        {
            return _wrappee.GetType();
        }

        public virtual bool TopUp(double subTotal)
        {
            return _wrappee.TopUp(subTotal);
        }

        public virtual bool WriteOff(double subTotal)
        {
            return _wrappee.WriteOff(subTotal);
        }
        
        private IAccount _wrappee;
    }
}