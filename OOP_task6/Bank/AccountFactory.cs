using System;

namespace Bank
{
    public class AccountFactory
    {
        public IAccount CreateAccount(Client client, double rate, double commission, double limit, DateTime exceedTime)
        {
            if (commission > 0)
            {
                if (exceedTime != DateTime.MinValue || limit < 0)
                    return null;
                return CreateCreditAccount(client, commission, rate, limit);
            }

            if (exceedTime > DateTime.MinValue)
                return CreateDepositAccount(client, rate, exceedTime);

            return CreateCurrentAccount(client, rate);
        }

        private IAccount CreateCurrentAccount(Client client, double rate)
        {
            CurrentAbstractAccount abstractAccount = new CurrentAbstractAccount(client, rate);
            return abstractAccount;
        }

        private IAccount CreateCreditAccount(Client client, double commission, double rate, double limit)
        {
            CreditAbstractAccount abstractAccount = new CreditAbstractAccount(client, commission, rate, limit);
            return abstractAccount;
        }

        private IAccount CreateDepositAccount(Client client, double rate, DateTime exceedTime)
        {
            DepositAbstractAccount abstractAccount = new DepositAbstractAccount(client, rate, exceedTime);
            return abstractAccount;
        }
        
    }
}