using System;
using Bank;
using Bank.Clients;

namespace OOP_task6
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new ClientBuilder()
                .SetName("Name")
                .SetSurname("Surname")
                .SetAddress("Address")
                .Build();
            Console.WriteLine(client.Name);
        }
    }
}