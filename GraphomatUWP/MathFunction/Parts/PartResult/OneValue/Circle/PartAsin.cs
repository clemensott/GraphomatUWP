using System;

namespace MathFunction
{
    class PartAsin : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "asin", "arcsin" };
        }

        public override double GetResult(double x)
        {
            return Math.Asin(valueRight.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartAsin();
        }
    }
}
