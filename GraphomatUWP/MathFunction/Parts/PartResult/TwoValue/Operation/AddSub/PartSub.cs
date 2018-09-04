using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartSub : PartAddSub
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.LeftHigher;
        }

        protected override string[] GetLowerLooks()
        {
            return new string[] { "-" };
        }

        public override void ChangeToPartSignIfSub(Parts parts)
        {
            parts[parts.IndexOf(this)] = new PartSign();
        }

        public override double GetResult(double x)
        {
            return valueLeft.GetResult(x) - valueRight.GetResult(x);
        }

        public override Part Clone()
        {
            return new PartSub();
        }
    }
}
