using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatUWP
{
    public class VerticalLine
    {
        private Vector2 point1, point2;

        public float X { get; set; }

        public float Y1 { get; set; }

        public float Y2 { get; set; }

        public Vector2 Point1
        {
            get
            {
                point1.X = X;
                point1.Y = Y1;

                return point1;
            }
        }

        public Vector2 Point2
        {
            get
            {
                point2.X = X;
                point2.Y = Y2;

                return point2;
            }
        }
        public VerticalLine()
        {
            X = Y1 = Y2 = 0;

            point1 = new Vector2();
            point2 = new Vector2();
        }

        public VerticalLine(float x, float y1, float y2)
        {
            X = x;
            Y1 = y1;
            Y2 = y2;

            point1 = new Vector2(x, y1);
            point2 = new Vector2(x, y2);
        }
    }
}
