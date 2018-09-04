using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatUWP
{
    class HorizontalLineTextRight : IHorizontalLineText
    {
        public IHorizontalLineText OtherIHorizontalLineText
        {
            get { return new HorizontalLineTextLeft(); }
        }

        public HorizontalPostition PositionMode { get { return HorizontalPostition.Right; } }

        public Vector2 GetBottomRightPoint(float x1, float x2, float y, float width, float height)
        {
            return new Vector2(x2 + width, y + height / 2);
        }

        public Vector2 GetTopLeftPoint(float x1, float x2, float y, float width, float height)
        {
            return new Vector2(x2, y - height / 2);
        }

        public Vector2 GetValuePoint(float x1, float x2, float y, float width, float height)
        {
            return new Vector2(x2, y - height / 2);
        }
    }
}
