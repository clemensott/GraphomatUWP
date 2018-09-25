using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartPow : PartPowRoot
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.LeftHigher;
        }

        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "^" ;
        }

        public override double GetResult(double x)
        {
            return Math.Pow(valueLeft.GetResult(x), valueRight.GetResult(x));
        }

        public override string ToEquationString()
        {
            return "^";
        }
    }
}
