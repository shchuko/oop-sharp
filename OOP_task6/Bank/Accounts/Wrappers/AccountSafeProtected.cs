namespace Bank.Accounts.Wrappers
{
    public class AccountSafeProtected : AccountWrapper
    {
        public AccountSafeProtected(IAccount source) : base(source)
        {
            
        }
        
        public override bool TransferTo(IAccount abstractAccount, double subTotal)
        {
            if (subTotal > NotVerifiedMaxLimit && !base.GetClient().IsVerified())
                return false;
            
            if (!base.GetClient().Equals(abstractAccount.GetClient()))
                return false;
            if (!base.WriteOff(subTotal))
                return false;
            if (!abstractAccount.TopUp(subTotal))
            {
                base.TopUp(subTotal);
                return false;
            }

            return true;
        }

        public override bool WriteOff(double subTotal)
        {
            return !(subTotal > NotVerifiedMaxLimit) && base.WriteOff(subTotal);
        }


        private const double NotVerifiedMaxLimit = 2000;
    }
}