using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartSign : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "-" ;
        }

        public override double GetResult(double x)
        {
            return -1 * valueRight.GetResult(x);
        }
    }
}
