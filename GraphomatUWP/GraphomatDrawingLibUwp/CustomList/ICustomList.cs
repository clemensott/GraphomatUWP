using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    interface ICustomList
    {
        IEnumerator<Vector2> GetEnumerator();
        IEnumerable<Vector2> GetValues(float beginX, float rangeX, float endX);
    }
}