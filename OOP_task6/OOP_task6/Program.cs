using System;
using Bank.Accounts;
using Bank.Accounts.Impl;
using Bank.Accounts.Wrappers;
using Bank.Clients;
using Bank.Requests;


namespace OOP_task6
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new ClientBuilder()
                .SetName("Danya")
                .SetSurname("Pupkin")
                .SetPassportId("88005553535")
                .SetAddress("Saint Petersburg")
                .Build();
            Console.WriteLine("Name: " + client.Name);
            Console.WriteLine("Surname: " + client.Surname);
            Console.WriteLine("Address: " + client.Address);
            Console.WriteLine("PassportId: " + client.PassportId);

            IAccount creditAccount = new AccountFactory().CreateAccount(client, 15, 120, 20000, DateTime.MinValue);
            IAccount depositAccount = new AccountFactory().CreateAccount(client, 3, 0, 0, DateTime.Now.AddYears(1));
            IAccount currentAccount = new AccountFactory().CreateAccount(client, 1, 0, 0, DateTime.MinValue);
            
            Console.WriteLine("Must be credit acc: " + creditAccount.GetAccountType());
            Console.WriteLine("Must be deposit acc: " + depositAccount.GetAccountType());
            Console.WriteLine("Must be current acc: " + currentAccount.GetAccountType());

            creditAccount.TopUp(300);
            depositAccount.TopUp(300);
            currentAccount.TopUp(300);
            
            Console.WriteLine("Total before: " + creditAccount.GetTotal());
            Request request = new TakeCommissionRequest(creditAccount);
            request.SetNextRequest(new UpdateByRateRequest(creditAccount));
            request.Execute();
            Console.WriteLine("Total after: " + creditAccount.GetTotal());

            IAccount depositAccountProtected = new AccountSafeProtected(depositAccount);
            Console.WriteLine(depositAccountProtected.TransferTo(creditAccount, 100)
                ? "Transfer 1 successful"
                : "Transfer 1 fail");

            Console.WriteLine(currentAccount.TransferTo(creditAccount, 100)
                ? "Transfer 2 successful"
                : "Transfer 2 fail");
            
            IAccount currentAccountProtected = new AccountSafeProtected(currentAccount);
            Console.WriteLine(currentAccountProtected.TransferTo(creditAccount, 100)
                ? "Transfer 3 successful"
                : "Transfer 3 fail");
        }
    }
}