using System;
using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    class PartVariable : PartValue
    {
        public PartVariable()
        {
        }

        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "x" ;
        }

        public override string ToString()
        {
            return base.ToEquationString();
        }

        public override double GetResult(double x)
        {
            return x;
        }
    }
}
