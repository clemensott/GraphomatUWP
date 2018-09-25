using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartAtan : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "atan";
            yield return "arctan";
        }

        public override double GetResult(double x)
        {
            return Math.Atan(valueRight.GetResult(x));
        }
    }
}
