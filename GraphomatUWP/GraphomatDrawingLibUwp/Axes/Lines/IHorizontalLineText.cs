using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatDrawingLibUwp
{
    enum HorizontalPostition { Right, Left }

    interface IHorizontalLineText
    {
        HorizontalPostition PositionMode { get; }

        IHorizontalLineText OtherIHorizontalLineText { get; }

        Vector2 GetValuePoint(float x1, float x2, float y, float width, float height);

        Vector2 GetTopLeftPoint(float x1, float x2, float y, float width, float height);

        Vector2 GetBottomRightPoint(float x1, float x2, float y, float width, float height);
    }
}
