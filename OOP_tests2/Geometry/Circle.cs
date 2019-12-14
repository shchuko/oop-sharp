using System;

namespace Geometry
{
    public class Circle
    {
        public void Print()
        {
            Console.WriteLine($"Center: ({_x}, {_y})");
            Console.WriteLine($"Radius: {GetRadius()}");
            Console.WriteLine($"Square: {GetSquare()}");
            Console.WriteLine($"Perimeter: {GetPerimeter()}");
        } 
        
        public double GetX()
        {
            return _x;
        }

        public double GetY()
        {
            return _y;
        }

        public double GetRadius()
        {
            return _r;
        }

        public double GetPerimeter()
        {
            return 2 * Math.PI * _r;
        }

        public double GetSquare()
        {
            return Math.PI * _r * _r;
        }

        public void Move(double dx, double dy)
        {
            _x += dx;
            _y += dy;
        }

        public void Move(Vector v)
        {
            _x += v.X;
            _y += v.Y;
        }

        public Circle(double x, double y, double r)
        {
            _x = x;
            _y = y;
            _r = r;
        }

        public override string ToString()
        {
            return $"Circle on ({_x}, {_y}) with radius {_r}";
        }

        private double _x;
        private double _y;
        private double _r;
    }
}