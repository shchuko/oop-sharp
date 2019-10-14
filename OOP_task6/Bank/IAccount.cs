namespace Bank
{
    public interface IAccount
    {
        bool TopUp(double subTotal);

        bool WriteOff(double subTotal);

        bool TransferTo(IAccount account, double subTotal);
        
        Client GetClient();
    }
}