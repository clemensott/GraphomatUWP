using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    class VerticalLineTextUnder : IVerticalLineText
    {
        public IVerticalLineText OtherVerticalLineText => new VerticalLineTextAbove();

        public VerticalPostition PositionMode => VerticalPostition.Under;

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
