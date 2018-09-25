using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartRoot : PartPowRoot
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.RightHigher;
        }

        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "r" ;
        }

        public override double GetResult(double x)
        {
            return Math.Pow(valueRight.GetResult(x), 1 / valueLeft.GetResult(x));
        }
    }
}
