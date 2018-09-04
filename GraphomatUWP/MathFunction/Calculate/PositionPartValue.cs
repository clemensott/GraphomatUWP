using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    class PositionPartValue
    {
        public int PartCalcIndex { get; private set; }

        public bool IsValue1 { get; private set; }

        public PositionPartValue(int index,bool isValue1)
        {
            PartCalcIndex = index;
            IsValue1 = isValue1;
        }
    }
}
