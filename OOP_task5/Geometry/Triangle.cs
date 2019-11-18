using System;

namespace Geometry
{
    [Serializable]
    public class Triangle
    {
        private Triangle()
        {
            
        }
        
        public Triangle(Point a, Point b, Point c)
        {
            _a = new Point(a);
            _b = new Point(b);
            _c = new Point(c);
        }

        public Triangle((double, double) a, (double, double) b, (double, double) c)
        {
            _a = new Point(a);
            _b = new Point(b);
            _c = new Point(c);
        }

        public Triangle(double x1, double y1, double x2, double y2, double x3, double y3)
        {
            _a = new Point(x1, y1);
            _b = new Point(x2, y2);
            _c = new Point(x3, y3);
        }

        public Point A
        {
            get => _a;
            set => _a = value;
        } 
        public Point B        
        {
            get => _b;
            set => _b = value;
        } 
        public Point C        
        {
            get => _c;
            set => _c = value;
        } 

        public override string ToString()
        {
            return $"[{_a}, {_b}, {_c}]";
        }

        private Point _a;
        private Point _b;
        private Point _c;
    }
}
