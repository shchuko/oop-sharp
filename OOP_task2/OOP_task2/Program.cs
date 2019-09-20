using System;

namespace OOP_task2
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] output = new MusicCatalogLib.ArgumentsParser().ParseAndExec(args);
            foreach (string s in output)
            {
                Console.WriteLine(s);
            }
        }
    }
}
