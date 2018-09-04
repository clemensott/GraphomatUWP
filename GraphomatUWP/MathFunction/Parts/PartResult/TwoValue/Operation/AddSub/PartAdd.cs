using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartAdd : PartAddSub
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.Same;
        }
        
        protected override string[] GetLowerLooks()
        {
            return new string[] { "+" };
        }

        public override void ChangeToPartSignIfSub(Parts parts)
        {
            parts.Remove(this);
        }

        public override double GetResult(double x)
        {
            return valueLeft.GetResult(x) + valueRight.GetResult(x);
        }

        public override Part Clone()
        {
            return new PartAdd();
        }
    }
}
