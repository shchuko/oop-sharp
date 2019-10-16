using Bank.Clients;

namespace Bank.Accounts
{
    public interface IAccount
    {
        bool TopUp(double subTotal);

        bool WriteOff(double subTotal);

        bool TransferTo(IAccount account, double subTotal);
        
        Client GetClient();

        double GetTotal();

        double GetRate();

        string GetType();
    }
}