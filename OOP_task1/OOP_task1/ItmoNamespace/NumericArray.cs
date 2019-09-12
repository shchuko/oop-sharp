using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ItmoNamespace
{
    public class NumericArray<T> : IEnumerable<T> where T : class, INumber, ICloneable<T>
    {

        public NumericArray()
        { }

        public NumericArray(String filepath, String pattern, IFactory<T> factory)
        {
            StreamReader sr = new StreamReader(filepath);
            String data = sr.ReadToEnd();
            foreach (Match match in Regex.Matches(data, pattern, RegexOptions.IgnoreCase))
            {
                this.Add(factory.createFromString(match.Value));
            }
        }

        public bool IsEmpty()
        {
            return list.Count() == 0;
        }

        public int Size()
        {
            return list.Count();
        }

        public void Add(T element)
        {
            list.Add(element);

            maxValCache = null;
            minValCache = null;

            lesserRequestResultCache = -1;
            greaterRequestResultCache = -1;
        }

        public T GetMin()
        {
            if (list.Count == 0)
            {
                return null;
            }

            if (minValCache != null)
            {
                return minValCache.GetClone();
            }

            T element = list[0];
            foreach (T e in list) {
                if (element.IsGreaterThan(e))
                {
                    element = e;
                }
            }
            return element.GetClone();
        }

        public T GetMax()
        {
            if (list.Count == 0)
            {
                return null;
            }

            if (maxValCache != null)
            {
                return maxValCache.GetClone();
            }

            T element = list[0];
            foreach (T e in list)
            {
                if (e.IsGreaterThan(element))
                {
                    element = e;
                }
            }
            return element.GetClone();
        }

        public int HowManyIsLesserThan(T element)
        {
            if (lesserRequestResultCache != -1 && lesserRequestElementCache.Equals(element))
            {
                return lesserRequestResultCache;
            }

            int counter = 0;
            foreach (T e in list)
            {
                if (element.IsGreaterThan(e))
                {
                    ++counter;
                }
            }
            lesserRequestElementCache = element;
            lesserRequestResultCache = counter;
            return counter;
        }

        public int HowManyIsGreaterThan(T element)
        {
            if (greaterRequestResultCache != -1 && greaterRequestElementCache.Equals(element))
            {
                return greaterRequestResultCache;
            }

            int counter = 0;
            foreach (T e in list)
            {
                if (e.IsGreaterThan(element))
                {
                    ++counter;
                }
            }
            greaterRequestElementCache = element;
            greaterRequestResultCache = counter;
            return counter;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (T e in list)
            {
                yield return e.GetClone();
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (T item in list)
            {
                sb.Append(item.ToString());
                sb.Append(' ');
            }
            sb.Remove(sb.Length - 1, 1);
            return sb.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            foreach (T e in list)
            {
                yield return e.GetClone();
            }
        }

        private List<T> list = new List<T>();

        private T minValCache = null;
        private T maxValCache = null;

        private T lesserRequestElementCache;
        private int lesserRequestResultCache = -1;

        private T greaterRequestElementCache;
        private int greaterRequestResultCache = -1;
    }
}
