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
        
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "+" ;
        }

        public override void ChangeToPartSignIfSub(Parts parts)
        {
            parts.Remove(this);
        }

        public override double GetResult(double x)
        {
            return valueLeft.GetResult(x) + valueRight.GetResult(x);
        }
    }
}
