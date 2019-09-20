using System;
using MusicCatalogLib;

namespace OOP_task2
{
    class Program
    {
        static void Main(string[] args)
        {
            ArgumentsParser argumentsParser = new ArgumentsParser(new MusicCatalogReader());
            string[] output = argumentsParser.ParseAndExec(args);
            foreach (string s in output)
            {
                Console.WriteLine(s);
            }
        }
    }
}
