using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartLg : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "lg" ;
        }

        public override double GetResult(double x)
        {
            return Math.Log10(valueRight.GetResult(x));
        }
    }
}
