using System;
using Bank.Clients;

namespace Bank.Accounts.Impl
{
    public class DepositAccount : AbstractAccount
    {
        internal DepositAccount(Client client, double rate, DateTime exceedTime) : base(client)
        {
            _rate = rate;
            _exceedTime = exceedTime;
        }

        public override bool WriteOff(double subTotal)
        {
            if (subTotal < 0 || Total - subTotal < 0 || _exceedTime > DateTime.Now)
                return false;

            Total -= subTotal;
            return true;
        }

        public override double GetRate()
        {
            return _rate;
        }

        public override string GetAccountType()
        {
            return "Deposit Account";
        }

        private double _rate;
        private DateTime _exceedTime;
    }
}