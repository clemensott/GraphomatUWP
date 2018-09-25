using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartTan : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "tan";
        }

        public override double GetResult(double x)
        {
            return Math.Tan(valueRight.GetResult(x));
        }
    }
}
