using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    struct PartResultPriority
    {
        public int TypePriority { get; private set; }

        public PriorityType PositionType { get; private set; }

        public PartResultPriority(int priority, PriorityType type)
        {
            TypePriority = priority;
            PositionType = type;
        }

        public int GetPositionPriority(int index, int count)
        {
            if (PositionType == PriorityType.Same) return count;

            return PositionType == PriorityType.RightHigher ? index : (count - index);
        }
    }
}
