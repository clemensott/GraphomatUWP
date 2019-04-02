using System.Collections.Generic;

namespace MathFunction
{
    class PartMult : PartMultDiv
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.Same;
        }

        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "•";
            yield return "*";
        }

        public override double GetResult(double x)
        {
            return valueLeft.GetResult(x) * valueRight.GetResult(x);
        }
    }
}
