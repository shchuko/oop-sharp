using System;
using System.Collections.Generic;
using ShopCatalog;

namespace OOP_task4
{
    class Program
    {
        static void Main(string[] args)
        {
            ShopCatalog.Service shopService = ShopCatalog.Manager.CreateService("");
            string[] result = shopService.ExecuteCommand("");
            foreach (string s in result)
            {
                Console.WriteLine(s);
            }
        }
    }
}