using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItmoNamespace;


namespace OOP_task1
{
    class Program
    {
        static void Main(string[] args)
        {
            NumericSet<Rational> numericSet = new NumericSet<Rational>();
            numericSet.Add(new Rational(1, 8));
            numericSet.Add(new Rational(1, 4));
            numericSet.Add(new Rational(1, 2));
            // place for 3/5
            numericSet.Add(new Rational(3, 4));
            numericSet.Add(new Rational(1, 4));     // won't be added, size == 4

            foreach (Rational rat in numericSet)
            {
                Console.Write(rat + " ");
            }
            Console.WriteLine("\n");

            Console.WriteLine("Size: " + numericSet.Size());
            Console.WriteLine("Max: " + numericSet.GetMax());
            Console.WriteLine("Min: " + numericSet.GetMin());

            Console.WriteLine("Greater than 3/5: " + numericSet.HowManyIsGreaterThan(new Rational(3, 5)));
            Console.WriteLine("Lesser than 3/5: " + numericSet.HowManyIsLesserThan(new Rational(3, 5)));

            Console.WriteLine("Greater than 1/2: " + numericSet.HowManyIsGreaterThan(new Rational(1, 2)));
            Console.WriteLine("Lesser than 1/2: " + numericSet.HowManyIsLesserThan(new Rational(1, 2)));
            Console.Read();
        }
    }
}
