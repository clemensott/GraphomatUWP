using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartLn : PartResultOneValue
    {
        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "ln" ;
        }

        public override double GetResult(double x)
        {
            return Math.Log(valueRight.GetResult(x));
        }
    }
}
