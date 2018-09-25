using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartDiv : PartMultDiv
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.LeftHigher;
        }

        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "÷";
            yield return "/";
        }

        public override double GetResult(double x)
        {
            return valueLeft.GetResult(x) / valueRight.GetResult(x);
        }
    }
}
