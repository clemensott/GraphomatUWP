using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    interface ICustomList : IEnumerable<Vector2>
    {
        IEnumerator<Vector2> GetEnumerator();
        IEnumerable<Vector2> GetValues(float beginX, float rangeX, float endX);
    }
}