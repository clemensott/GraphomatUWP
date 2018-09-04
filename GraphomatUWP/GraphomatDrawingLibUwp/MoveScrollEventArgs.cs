using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace GraphomatDrawingLibUwp
{
    class MoveScrollEventArgs
    {

        public float OverRender { get { return MoveScollManager.OverRender; } }

        public ViewDimensions ViewDimensions { get; private set; }

        public MoveScrollEventArgs(ViewDimensions viewDimensions)
        {
            ViewDimensions = viewDimensions;
        }
    }
}
