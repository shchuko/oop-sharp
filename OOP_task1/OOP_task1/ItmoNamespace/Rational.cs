using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItmoNamespace
{
    public class Rational : ItmoNamespace.ICloneable<Rational>, INumber
    {
        public Rational(int numerator, int denominator) 
        {
            this.Set(numerator, denominator);
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

        public bool Equals(Rational obj)
        {
            return obj.denominator == this.denominator && obj.numerator == this.numerator;
        }

        public Rational GetClone()
        {
            return new Rational(numerator, denominator);
        }

        private int numerator;
        private int denominator;
    }
}
