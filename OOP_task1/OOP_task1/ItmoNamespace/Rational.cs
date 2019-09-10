using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItmoNamespace
{
    public class Rational : ICloneable<Rational>, INumber, IEquatable<Rational>, ISummable<Rational>
    {
        public Rational(int numerator, int denominator) 
        {
            this.Set(numerator, denominator);
        }

        public Rational(String str)
        {
            String[] temp = str.Split('/');
            numerator = Int32.Parse(temp[0]);
            denominator = Int32.Parse(temp[1]);
        }

        

        public int GetNumerator()
        {
            return numerator;
        }

        public int GetDenominator()
        {
            return denominator;
        }

        public double GetDouble()
        {
            return (double)numerator / denominator;
        }
        
        public int GetInt()
        {
            return (int)Math.Round(GetDouble(), MidpointRounding.AwayFromZero);
        }

        public bool IsGreaterThan(Rational rational)
        {
            return this.GetDouble() > rational.GetDouble();
        }

        public bool IsGreaterThan(INumber objToCompare)
        {
            return this.GetDouble() > objToCompare.GetDouble();
        }

        public void SetNumerator(int numerator)
        {
            this.numerator = numerator;
        }

        public void SetDenominator(int denominator)
        {
            if (denominator == 0)
            {
                throw new System.DivideByZeroException();
            }
        }
        public void Set(int numerator, int denominator)
        {
            if (denominator == 0)
            {
                throw new System.DivideByZeroException();
            }
            this.numerator = numerator;
            this.denominator = denominator;
        }

        public override string ToString()
        {
            return numerator + "/" + denominator;
        }

        public Rational GetClone()
        {
            return new Rational(numerator, denominator);
        }
        public bool Equals(Rational other)
        {
            return other.denominator == this.denominator && other.numerator == this.numerator;
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() == this.GetType())
            {
                return Equals((Rational)obj);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            var hashCode = -671859081;
            hashCode = hashCode * -1521134295 + numerator.GetHashCode();
            hashCode = hashCode * -1521134295 + denominator.GetHashCode();
            return hashCode;
        }

        public static Rational operator + (Rational left, Rational right)
        {
            if (left.denominator == right.denominator)
            {
                return new Rational(left.numerator + right.numerator, left.denominator);
            }

            int newNumerator = left.numerator * right.denominator + right.numerator * left.denominator;
            int newDenominator = left.denominator * right.denominator;
            return new Rational(newNumerator, newDenominator);
        }

        public Rational Sum(Rational rational)
        {
            return this + rational;
        }

        private int numerator;
        private int denominator;
    }
}
