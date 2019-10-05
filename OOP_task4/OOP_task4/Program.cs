using System;
using System.Collections.Generic;
using ShopCatalog;

namespace OOP_task4
{
    class Program
    {
        static void Main(string[] args)
        {
            string mariaDbServiceArgs = "server=localhost;port=3306;user id=shopAdmin; password=password; " +
                                      "database=ShopDB; SslMode=none";
            
            Service service = Manager.CreateService(ServiceEngineTypes.MariaDBEngineType, mariaDbServiceArgs);
            
            string[] result = service.ExecuteCommand("");
            foreach (string s in result)
            {
                Console.WriteLine(s);
            }
        }
    }
}