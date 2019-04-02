using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    class HorizontalLineTextRight : IHorizontalLineText
    {
        public IHorizontalLineText OtherIHorizontalLineText => new HorizontalLineTextLeft();

        public HorizontalPosition PositionMode => HorizontalPosition.Right;

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
