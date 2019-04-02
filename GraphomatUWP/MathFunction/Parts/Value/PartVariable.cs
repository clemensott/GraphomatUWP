using System.Collections.Generic;

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
