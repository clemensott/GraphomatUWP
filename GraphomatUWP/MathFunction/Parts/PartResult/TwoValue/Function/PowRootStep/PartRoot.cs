using System;

namespace MathFunction
{
    class PartRoot : PartPowRoot
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.RightHigher;
        }

        protected override string[] GetLowerLooks()
        {
            return new string[] { "r" };
        }

        public override double GetResult(double x)
        {
            return Math.Pow(valueRight.GetResult(x), 1 / valueLeft.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartRoot();
        }
    }
}
