using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatUWP
{
    interface ILineText
    {
        Vector2 ValuePoint { get; }

        Vector2 TopLeftPoint { get; }

        Vector2 BottomRightPoint { get; }

        void ChangePosition();
    }
}
