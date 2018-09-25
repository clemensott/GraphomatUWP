using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartSin : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "sin";
        }

        public override double GetResult(double x)
        {
            return Math.Sin(valueRight.GetResult(x));
        }
    }
}
