using System.Reflection;
using Bank.Accounts;
using Bank.Accounts.Impl;

namespace Bank.Requests
{
    public class TakeCommissionRequest : Request
    {
        public TakeCommissionRequest(IAccount account) : base(account)
        {
        }

        internal override void ExecuteRequest()
        {
            if (!(GetAccount() is CreditAccount))
            {
                return;
            }

            CreditAccount account = (CreditAccount) GetAccount();
            account.WriteOff(account.Commission);
        }
    }
}