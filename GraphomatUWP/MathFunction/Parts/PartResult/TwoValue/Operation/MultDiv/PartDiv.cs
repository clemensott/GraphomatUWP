using System;

namespace MathFunction
{
    class PartDiv : PartMultDiv
    {
        public override PriorityType GetPriorityType()
        {
            return PriorityType.LeftHigher;
        }

        protected override string[] GetLowerLooks()
        {
            return new string[] { "÷", "/" };
        }

        public override double GetResult(double x)
        {
            return valueLeft.GetResult(x) / valueRight.GetResult(x);
        }

        public override Part Clone()
        {
            return new PartDiv();
        }
    }
}
