using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartAbs : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "abs" ;
        }

        public override double GetResult(double x)
        {
            return Math.Abs(valueRight.GetResult(x));
        }
    }
}
