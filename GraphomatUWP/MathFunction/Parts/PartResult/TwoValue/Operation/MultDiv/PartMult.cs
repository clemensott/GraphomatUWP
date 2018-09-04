using System;

namespace MathFunction
{
    class PartMult : PartMultDiv
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.Same;
        }

        protected override string[] GetLowerLooks()
        {
            return new string[] { "•", "*" };
        }

        public override double GetResult(double x)
        {
            return valueLeft.GetResult(x) * valueRight.GetResult(x);
        }

        public override Part Clone()
        {
            return new PartMult();
        }
    }
}
