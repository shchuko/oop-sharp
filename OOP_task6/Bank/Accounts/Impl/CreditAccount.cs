using Bank.Clients;

namespace Bank.Accounts.Impl
{
    public class CreditAccount : AbstractAccount
    {
        internal CreditAccount(Client client, double commission, double rate, double limit) : base(client)
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

        public override double GetRate()
        {
            return _rate;
        }

        public double Commission => _commission;

        public double Rate => _rate;

        private double _commission;
        private double _rate;
        private double _limit;
    }
}