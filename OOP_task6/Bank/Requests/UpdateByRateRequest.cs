using Bank.Accounts;
using Bank.Accounts.Impl;

namespace Bank.Requests
{
    public class UpdateByRateRequest : Request
    {
        public UpdateByRateRequest(IAccount account) : base(account)
        {
        }

        internal override void ExecuteRequest()
        {
            IAccount account = GetAccount();
            if (GetAccount() is CreditAccount)
            {
                account.WriteOff(account.GetTotal() * account.GetRate() / 100);
                return;
            }

            if (GetAccount() is CurrentAccount || GetAccount() is DepositAccount)
            {
                account.TopUp(account.GetTotal() * account.GetRate() / 100);
                return;
            }
        }
    }
}