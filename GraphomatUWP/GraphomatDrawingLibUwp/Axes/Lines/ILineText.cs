using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    interface ILineText
    {
        Vector2 ValuePoint { get; }

        Vector2 TopLeftPoint { get; }

        Vector2 BottomRightPoint { get; }

        void ChangePosition();
    }
}
