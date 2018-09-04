using System;

namespace MathFunction
{
    class PartSin : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "sin" };
        }

        public override double GetResult(double x)
        {
            return Math.Sin(valueRight.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartSin();
        }
    }
}
