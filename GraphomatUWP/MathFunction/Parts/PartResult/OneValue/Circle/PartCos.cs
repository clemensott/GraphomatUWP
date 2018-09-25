using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartCos : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "cos";
        }

        public override double GetResult(double x)
        {
            return Math.Cos(valueRight.GetResult(x));
        }
    }
}
