using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartAsin : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "asin";
            yield return "arcsin";
        }

        public override double GetResult(double x)
        {
            return Math.Asin(valueRight.GetResult(x));
        }
    }
}
