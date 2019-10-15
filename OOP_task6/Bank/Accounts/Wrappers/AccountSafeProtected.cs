namespace Bank.Accounts.Wrappers
{
    public class AccountSafeProtected : AccountWrapper
    {
        public AccountSafeProtected(IAccount source) : base(source)
        {
            
        }
        
        public override bool TransferTo(IAccount abstractAccount, double subTotal)
        {
            if (subTotal > NotVerifiedMaxLimit && !GetClient().IsVerified())
                return false;
            
            return base.TransferTo(abstractAccount, subTotal);
        }

        public override bool WriteOff(double subTotal)
        {
            return !(subTotal > NotVerifiedMaxLimit) && base.WriteOff(subTotal);
        }


        private const double NotVerifiedMaxLimit = 2000;
    }
}