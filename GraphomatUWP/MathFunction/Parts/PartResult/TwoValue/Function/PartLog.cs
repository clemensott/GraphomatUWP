using System;

namespace MathFunction
{
    class PartLog : PartResultTwoValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "log" };
        }

        public override int GetPriorityValue()
        {
            return 5;
        }

        public override PriorityType GetPriorityType()
        {
            return PriorityType.RightHigher;
        }

        public override double GetResult(double x)
        {
            return Math.Log(valueLeft.GetResult(x), valueLeft.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartLog();
        }
    }
}
