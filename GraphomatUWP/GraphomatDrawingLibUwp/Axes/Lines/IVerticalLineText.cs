using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatDrawingLibUwp
{
    enum VerticalPostition { Above, Under }

    interface IVerticalLineText
    {
        VerticalPostition PositionMode { get; }

        IVerticalLineText OtherVerticalLineText { get; }

        Vector2 GetValuePoint(float x, float y1, float y2, float width, float height);

        Vector2 GetTopLeftPoint(float x, float y1, float y2, float width, float height);

        Vector2 GetBottomRightPoint(float x, float y1, float y2, float width, float height);
    }
}
