using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatUWP
{
    class HorizontalLineTextLeft : IHorizontalLineText
    {
        public IHorizontalLineText OtherIHorizontalLineText
        {
            get { return new HorizontalLineTextRight(); }
        }

        public HorizontalPostition PositionMode { get { return HorizontalPostition.Left; } }

        public Vector2 GetBottomRightPoint(float x1, float x2, float y, float width, float height)
        {
            return new Vector2(x1, y + height / 2);
        }

        public Vector2 GetTopLeftPoint(float x1, float x2, float y, float width, float height)
        {
            return new Vector2(x1 - width, y - height / 2);
        }

        public Vector2 GetValuePoint(float x1, float x2, float y, float width, float height)
        {
            return new Vector2(x1 - width, y - height / 2);
        }
    }
}
