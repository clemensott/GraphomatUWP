using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    class HorizontalLineTextLeft : IHorizontalLineText
    {
        public IHorizontalLineText OtherIHorizontalLineText => new HorizontalLineTextRight();

        public HorizontalPosition PositionMode => HorizontalPosition.Left;

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
