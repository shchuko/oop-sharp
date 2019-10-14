using System;

namespace Bank
{
    public class CreditAbstractAccount : AbstractAccount
    {
        internal CreditAbstractAccount(Client client, double commission, double rate, double limit) : base(client)
        {
            _commission = commission;
            _rate = rate;
            _limit = limit;
        }

        public override bool WriteOff(double subTotal)
        {
            if (subTotal < 0 || Total + _limit - subTotal < 0)
                return false;

            Total -= subTotal;
            return true;
        }
        
        private double _commission;
        private double _rate;
        private double _limit;
    }
}