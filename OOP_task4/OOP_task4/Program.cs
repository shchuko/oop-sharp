using System;
using ShopCatalog;

namespace OOP_task4
{
    class Program
    {
        static void Main(string[] args)
        {
            string mariaDbServiceArgs = "server=localhost;port=3306;user id=shopAdmin; password=password; " +
                                      "database=ShopDB; SslMode=none";
            
//            Service service = Manager.CreateService(ServiceEngineTypes.MariaDBEngineType, mariaDbServiceArgs);

            string csvConnectString = "shopData=/home/shchuko/csvdata/shopData.csv;" +
                                      "productData=/home/shchuko/csvdata/productData.csv";
            Service service = Manager.CreateService(ServiceEngineTypes.CsvEngineType, csvConnectString);

            while (true)
            {
                Console.Write(">> ");
                string command = Console.In.ReadLine();
                if (command.Contains("exit"))
                {
                    Console.WriteLine("exit");
                    break;
                }

                string[] result = service.ExecuteCommand(command);
                foreach (string s in result)
                {
                    Console.WriteLine(s);
                }
            }
            
        }
    }
}