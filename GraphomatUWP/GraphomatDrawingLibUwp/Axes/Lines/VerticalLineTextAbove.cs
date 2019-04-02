using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    class VerticalLineTextAbove : IVerticalLineText
    {
        public IVerticalLineText OtherVerticalLineText => new VerticalLineTextUnder();

        public VerticalPostition PositionMode => VerticalPostition.Above;

        public Vector2 GetBottomRightPoint(float x, float y1, float y2, float width, float height)
        {
            return new Vector2(x + width / 2, y1);
        }

        public Vector2 GetTopLeftPoint(float x, float y1, float y2, float width, float height)
        {
            return new Vector2(x - width / 2, y1 - height);
        }

        public Vector2 GetValuePoint(float x, float y1, float y2, float width, float height)
        {
            return new Vector2(x - width / 2, y1 - height);
        }
    }
}
