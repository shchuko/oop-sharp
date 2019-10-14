using System;

namespace Bank
{
    public class DepositAbstractAccount : AbstractAccount
    {
        internal DepositAbstractAccount(Client client, double rate, DateTime exceedTime) : base(client)
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
        
        private double _rate;
        private DateTime _exceedTime;
    }
}