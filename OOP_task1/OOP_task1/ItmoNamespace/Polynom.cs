using ItmoNamespace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItmoNamespace
{
    public class Polynom<T> where T : class, INumber, ICloneable<T>, ISummable<T>
    {
        Polynom()
        {

        }

        public Polynom(NumericArray<T> numericArray)
        {
            list = new List<T>(numericArray);
        }

        public Polynom(ICollection<T> collection)
        {
            list = new List<T>(collection);
        }

        public Polynom(Polynom<T> polynom)
        {
            list = new List<T>(polynom.list);
        }

        public int GetExponent()
        {
            return list.Count - 1;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            int counter = list.Count - 1;
            foreach (T element in list)
            {
                stringBuilder.Append("(" + element + ")*x^" + counter + " + ");
                --counter;
            }
            if (stringBuilder.Length != 0)
            {
                stringBuilder.Remove(stringBuilder.Length - 7, 7);
            }
            return stringBuilder.ToString();
        }

        public static Polynom<T> operator + (Polynom<T> left, Polynom<T> right) {
            // left must have greater exponent
            if (left.GetExponent() < right.GetExponent()) {
                Polynom<T> temp = left;
                left = right;
                right = temp;
            }
            int leftSize = left.list.Count;
            int rightSize = right.list.Count;

            Polynom<T> toCreate = new Polynom<T>(left);
            for (int i = 0; i < rightSize; ++i)
            {
                toCreate.list[i + leftSize - rightSize] = left.list[i + leftSize - rightSize].Sum(right.list[i]);
            }
            return toCreate;
        }

        private List<T> list;
    }
}
