using System;
using System.IO;
using ShopCatalog;
using IniParser;
using IniParser.Model;

namespace OOP_task4
{
    class Program
    {
        static void Main(string[] args)
        {
            StartApp(args[0]);
        }

        private static void StartApp(string propertyFilepath)
        {
            (string, string) properties;
            try
            {
                properties = ParsePropertyFile(propertyFilepath);
            }
            catch (Exception)
            {
                Console.WriteLine("Property file reading error");
                return;
            }
            
            Service service;
            try
            {
                service = Manager.CreateService(properties.Item1, properties.Item2);
            }
            catch (Exception)
            {
                Console.WriteLine("Creation service error");
                return;
            }

            Console.WriteLine("Connection successful. Type 'exit' to exit");
            while (true)
            {
                Console.Write(">> ");
                string command = Console.In.ReadLine();
                if ("exit".Equals(command) || command == null) 
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

        private static (string, string) ParsePropertyFile(string filepath)
        {
            IniData data = new StringIniParser().ParseString(File.ReadAllText(filepath));
            string driver = data["CATALOG_PROPERTIES"]["driver"];
            string connectionString = data["CATALOG_PROPERTIES"]["connection-string"];
            
            return (driver, connectionString);
        }
    }
}