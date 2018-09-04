using System;

namespace MathFunction
{
    class PartPow : PartPowRoot
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.LeftHigher;
        }

        protected override string[] GetLowerLooks()
        {
            return new string[] { "^" };
        }

        public override double GetResult(double x)
        {
            return Math.Pow(valueLeft.GetResult(x), valueRight.GetResult(x));
        }

        public override string ToEquationString()
        {
            return "^";
        }

        public override Part Clone()
        {
            return new PartRoot();
        }
    }
}
