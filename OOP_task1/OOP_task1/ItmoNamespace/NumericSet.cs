using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ItmoNamespace
{
    public class NumericSet<T> where T : INumber
    {
        public bool IsEmpty()
        {
            return size == 0;
        }

        public int Size()
        {
            return size;
        }

        public bool Contains(T element)
        {
            return Find(element) != null;
        }

        public void Add(T element)
        {
            if (Find(element) != null)
            {
                return;
            }

            Node temp = Emplace(element);
            UpdateMinMax(temp);
            ++size;
        }

        public int HowManyIsLesserThan(T element)
        {
            if (lesserRequestResultCache != -1 && lesserRequestElementCache.Equals(element))
            {
                return lesserRequestResultCache;
            }

            int lesserElementsCounter = 0;
            Node temp = dataHead;
            while (temp != null && element.IsGreaterThan(temp.data))
            {
                temp = temp.next;
                ++lesserElementsCounter;
            }

            lesserRequestElementCache = element;
            lesserRequestResultCache = lesserElementsCounter;
            return lesserElementsCounter;
        }
        public int HowManyIsGreaterThan(T element)
        {
            if (greaterRequestResultCache != -1 && greaterRequestElementCache.Equals(element))
            {
                return greaterRequestResultCache;
            }

            int lesserOrEqualElementsCounter = 0;
            Node temp = dataHead;
            while (temp != null && (element.IsGreaterThan(temp.data) || element.Equals(temp.data)))
            {
                temp = temp.next;
                ++lesserOrEqualElementsCounter;
            }

            int greaterElementsCounter = size - lesserOrEqualElementsCounter;

            greaterRequestElementCache = element;
            greaterRequestResultCache = greaterElementsCounter;
            return greaterElementsCounter;
        }

        private int size = 0;
        private Node dataHead = null;
        
        private Node minValCache = null;
        private Node maxValCache = null;

        private T lesserRequestElementCache;
        private int lesserRequestResultCache = -1;

        private T greaterRequestElementCache;
        private int greaterRequestResultCache = -1;

        private Node Find(T element)
        {
            Node temp = dataHead;
            while (temp != null && !temp.data.Equals(element)) {
                temp = temp.next;
            }
            return temp;
        }

        private Node Emplace(T element)
        {
            if (dataHead == null)
            {
                dataHead = new Node(element);
                return dataHead;
            }

            Node nodeToCompare = dataHead;
            Node nodeToComparePrev = null;

            while (nodeToCompare.next != null && element.IsGreaterThan(nodeToCompare.data))
            {
                nodeToComparePrev = nodeToCompare;
                nodeToCompare = nodeToCompare.next;
            }

            Node insertedElementNode = new Node(element, nodeToCompare);
            if (nodeToComparePrev == null)
            {
                dataHead = insertedElementNode;
            }
            else
            {
                nodeToComparePrev.next = insertedElementNode;
            }
            return insertedElementNode;
        }

        private void UpdateMinMax(Node element)
        {
            if (maxValCache == null || element.data.IsGreaterThan(maxValCache.data))
            {
                maxValCache = element;
            }

            if (minValCache == null || !element.data.IsGreaterThan(minValCache.data))
            {
                minValCache = element;
            }
        }

        private class Node
        {
            public T data;
            public Node next = null;

            public Node(T data, Node next)
            {
                this.data = data;
                this.next = next;
            }

            public Node(T data)
            {
                this.data = data;
            }
        }

    }
}
