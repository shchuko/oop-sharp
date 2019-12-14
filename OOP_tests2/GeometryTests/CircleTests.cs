using System;
using System.IO;
using NUnit.Framework;
using Geometry;

namespace GeometryTests
{
    public class CircleTests
    {
        private const double Tolerance = 0.0001;
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void CircleGetXTest()
        {
            double resultX = 1.5;
            Assert.That(new Circle(resultX, 1, 1).GetX(), Is.EqualTo(resultX).Within(Tolerance));
        }

        [Test]
        public void CircleGetYTest()
        {
            double resultY = 2.5;
            Assert.That(new Circle(1, resultY, 1).GetY(), Is.EqualTo(resultY).Within(Tolerance));

        }

        [Test]
        public void CircleRadiusTest()
        {
            double resultRadius = 10.5;
            Assert.That(new Circle(1, 1, resultRadius).GetRadius(), Is.EqualTo(resultRadius).Within(Tolerance));
        }

        [Test]
        public void CircleGetSquareTest()
        {
            double radiusValue = 10.5;
            double squareResult = radiusValue * radiusValue * Math.PI;
            
            Assert.That(new Circle(1, 1, radiusValue).GetSquare(), Is.EqualTo(squareResult).Within(Tolerance));
        }

        [Test]
        public void CircleGetPerimeterTest()
        {
            double radiusValue = 10.5;
            double perimeterResult = 2 * Math.PI * radiusValue;
            
            Assert.That(new Circle(1, 1, radiusValue).GetPerimeter(), Is.EqualTo(perimeterResult).Within(Tolerance));
        }

        [Test]
        public void CircleMoveTest()
        {
            double xBefore = 1.3;
            double dx = -6.7;
            double resultX = xBefore + dx;

            double yBefore = 2.5;
            double dy = 5.4;
            double resultY = yBefore + dy;
            
            var circle = new Circle(xBefore, yBefore, 1);
            circle.Move(dx, dy);
            
            Assert.That(new Circle(resultX, 1, 1).GetX(), Is.EqualTo(resultX).Within(Tolerance));
            Assert.That(new Circle(1, resultY, 1).GetY(), Is.EqualTo(resultY).Within(Tolerance));
        }
        
        [Test]
        public void CircleMoveVectorTest()
        {
            double xBefore = 1.3;
            double dx = -6.7;
            double resultX = xBefore + dx;

            double yBefore = 2.5;
            double dy = 5.4;
            double resultY = yBefore + dy;
            
            var circle = new Circle(xBefore, yBefore, 1);
            var vector = new Vector(dx, dy);

            circle.Move(vector);
            
            Assert.That(new Circle(resultX, 1, 1).GetX(), Is.EqualTo(resultX).Within(Tolerance));
            Assert.That(new Circle(1, resultY, 1).GetY(), Is.EqualTo(resultY).Within(Tolerance));
        }

        [Test]
        public void CirclePrintTest()
        {
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);
                double x = 1;
                double y = 2;
                double r = 3;
                
                var circle = new Circle(x, y, r);
                circle.Print();

                double square = r * r * Math.PI;
                double perimeter = 2 * Math.PI * r;
                
                string expected = $"Center: ({x}, {y}){Environment.NewLine}Radius: {r}{Environment.NewLine}Square:" + 
                                  $" {square}{Environment.NewLine}Perimeter: {perimeter}{Environment.NewLine}";
                Assert.AreEqual(expected, sw.ToString());
            }
        }

        [Test]
        public void CircleToStringTest()
        {
            double x = 1;
            double y = 2;
            double r = 3;
            
            string expected = $"Circle on ({x}, {y}) with radius {r}";
            
            Assert.AreEqual(expected, new Circle(x, y, r).ToString());
        }
        
        
    }
}