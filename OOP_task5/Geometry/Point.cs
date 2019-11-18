using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Geometry
{
    [Serializable]
    public class Point
    {
        public Point() : this(0, 0)
        {
        }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point((double, double) cords)
        {
            var (x, y) = cords;
            X = x;
            Y = y;
        }
        
        public Point(Point p)
        {
            X = p.X;
            Y = p.Y;
        }
        public double X { get; set; }
        public double Y { get; set; }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}