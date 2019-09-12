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
            Console.WriteLine("Numetic Array of Rational: ");
            Console.WriteLine(numericArray);

            Console.WriteLine("Size: " + numericArray.Size());
            Console.WriteLine("Max: " + numericArray.GetMax());
            Console.WriteLine("Min: " + numericArray.GetMin());

            Console.WriteLine("Greater than 3/5 (count): " + numericArray.HowManyIsGreaterThan(new Rational(3, 5)));
            Console.WriteLine("Lesser than 3/5 (count): " + numericArray.HowManyIsLesserThan(new Rational(3, 5)));

            Console.WriteLine("Greater than 1/2 (count): " + numericArray.HowManyIsGreaterThan(new Rational(1, 2)));
            Console.WriteLine("Lesser than 1/2 (count): " + numericArray.HowManyIsLesserThan(new Rational(1, 2)));

            String filepath = "data.txt";
            String pattern = "[0-9]+/[0-9]+";
            RationalFactory rationalFactory = new RationalFactory();
            NumericArray<Rational> numericArray2 = new NumericArray<Rational>(filepath, pattern, rationalFactory);

            Console.WriteLine();
            Console.WriteLine("NumericArray2, created from file: ");
            Console.WriteLine(numericArray2);

            Polynom<Rational> polynom1 = new Polynom<Rational>(numericArray);
            Polynom<Rational> polynom2 = new Polynom<Rational>(numericArray2);

            Console.WriteLine();
            Console.WriteLine("Polynom1:");
            Console.WriteLine(polynom1);
            Console.WriteLine("Polynom2:");
            Console.WriteLine(polynom2);
            Console.WriteLine("Sum:");
            Console.WriteLine(polynom1 + polynom2);

            Console.WriteLine();
            Console.WriteLine("Trying to operate with rational with zero denominator:");
            try
            {
                Rational r = new Rational("3/0");
                Console.WriteLine(r);
                Console.WriteLine(r.GetDouble());
            } catch (Exception ex)
            {
                Console.WriteLine("Exception catched: " + ex.ToString());
            }
            Console.Read();
        }
    }
}
