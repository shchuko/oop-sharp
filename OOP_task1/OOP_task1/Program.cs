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
            NumericArray<Rational> numericArray = new NumericArray<Rational>();
            numericArray.Add(new Rational(1, 8));
            numericArray.Add(new Rational(1, 4));
            numericArray.Add(new Rational(1, 2));
            // place for 3/5
            numericArray.Add(new Rational(3, 4));
            numericArray.Add(new Rational(1, 4));    

            foreach (Rational rat in numericArray)
            {
                Console.Write(rat + " ");
            }
            Console.WriteLine("\n");

            Console.WriteLine("Size: " + numericArray.Size());
            Console.WriteLine("Max: " + numericArray.GetMax());
            Console.WriteLine("Min: " + numericArray.GetMin());

            Console.WriteLine("Greater than 3/5: " + numericArray.HowManyIsGreaterThan(new Rational(3, 5)));
            Console.WriteLine("Lesser than 3/5: " + numericArray.HowManyIsLesserThan(new Rational(3, 5)));

            Console.WriteLine("Greater than 1/2: " + numericArray.HowManyIsGreaterThan(new Rational(1, 2)));
            Console.WriteLine("Lesser than 1/2: " + numericArray.HowManyIsLesserThan(new Rational(1, 2)));

            NumericArray<Rational> numericArray2 = new NumericArray<Rational>();
            numericArray2.Add(new Rational(1, 4));
            numericArray2.Add(new Rational(1, 8));
            numericArray2.Add(new Rational(1, 2));
            numericArray2.Add(new Rational(1, 4));
            numericArray2.Add(new Rational(3, 4));

            Console.WriteLine(new Polynom<Rational>(numericArray));
            Console.WriteLine(new Polynom<Rational>(numericArray2));
            Console.WriteLine(new Polynom<Rational>(numericArray2) + new Polynom<Rational>(numericArray));


            Console.Read();

           
        }
    }
}
