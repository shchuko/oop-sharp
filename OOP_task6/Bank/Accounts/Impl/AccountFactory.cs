using System;
using Bank.Clients;

namespace Bank.Accounts.Impl
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
            CurrentAccount account = new CurrentAccount(client, rate);
            return account;
        }

        private IAccount CreateCreditAccount(Client client, double commission, double rate, double limit)
        {
            CreditAccount account = new CreditAccount(client, commission, rate, limit);
            return account;
        }

        private IAccount CreateDepositAccount(Client client, double rate, DateTime exceedTime)
        {
            DepositAccount account = new DepositAccount(client, rate, exceedTime);
            return account;
        }
        
    }
}