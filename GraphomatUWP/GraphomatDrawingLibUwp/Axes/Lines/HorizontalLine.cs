using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatDrawingLibUwp
{
    class HorizontalLine
    {
        public float X1 { get; set; }

        public float X2 { get; set; }

        public float Y { get; set; }

        public Vector2 Point1 { get { return new Vector2(X1, Y); } }

        public Vector2 Point2 { get { return new Vector2(X2, Y); } }

        public HorizontalLine()
        {
            X1 = X2 = Y = 0;
        }

        public HorizontalLine(float x1, float x2, float y)
        {
            X1 = x1;
            X2 = x2;
            Y = y;
        }
    }
}
