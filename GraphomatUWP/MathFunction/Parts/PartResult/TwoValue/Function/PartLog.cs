using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartLog : PartResultTwoValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "log";
        }

        public override int GetPriorityValue()
        {
            return 5;
        }

        public override PriorityType GetPriorityType()
        {
            return PriorityType.RightHigher;
        }

        public override double GetResult(double x)
        {
            return Math.Log(valueLeft.GetResult(x), valueLeft.GetResult(x));
        }
    }
}
