using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartAcos : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "acos";
            yield return "arccos";
        }

        public override double GetResult(double x)
        {
            return Math.Acos(valueRight.GetResult(x));
        }
    }
}
