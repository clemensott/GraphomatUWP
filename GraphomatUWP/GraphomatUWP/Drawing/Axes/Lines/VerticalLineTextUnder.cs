using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatUWP
{
    class VerticalLineTextUnder : IVerticalLineText
    {
        public IVerticalLineText OtherVerticalLineText
        {
            get { return new VerticalLineTextAbove(); }
        }

        public VerticalPostition PositionMode { get { return VerticalPostition.Under; } }

        public Vector2 GetBottomRightPoint(float x, float y1, float y2, float width, float height)
        {
            return new Vector2(x + width / 2, y2 + height);
        }

        public Vector2 GetTopLeftPoint(float x, float y1, float y2, float width, float height)
        {
            return new Vector2(x - width / 2, y2);
        }

        public Vector2 GetValuePoint(float x, float y1, float y2, float width, float height)
        {
            return new Vector2(x - width / 2, y2);
        }
    }
}
