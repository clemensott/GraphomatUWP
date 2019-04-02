using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    enum HorizontalPosition { Right, Left }

    interface IHorizontalLineText
    {
        HorizontalPosition PositionMode { get; }

        IHorizontalLineText OtherIHorizontalLineText { get; }

        Vector2 GetValuePoint(float x1, float x2, float y, float width, float height);

        Vector2 GetTopLeftPoint(float x1, float x2, float y, float width, float height);

        Vector2 GetBottomRightPoint(float x1, float x2, float y, float width, float height);
    }
}
