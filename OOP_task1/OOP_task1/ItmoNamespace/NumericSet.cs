using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItmoNamespace
{
    public class NumericSet<T>
    {
        public void Add(T element)
        {
            if (size == capacity)
            {
                IncreaseCapacity();
            }

            data[size++] = element;
        }



        private static int DEFAULT_CAPACITY = 10;

        private int size = 0;
        private int capacity = DEFAULT_CAPACITY;
        private T[] data = new T[DEFAULT_CAPACITY];
        private T minValCache;
        private T maxValCache;

        private void IncreaseCapacity()
        {
            T[] temp = new T[capacity * 2];
            capacity *= 2;

            for (int i = 0; i < size; ++i)
            {
                temp[i] = data[i];
            }
            data = temp;
        }

    }
}
