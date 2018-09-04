using System;

namespace MathFunction
{
    class PartAbs : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "abs" };
        }

        public override double GetResult(double x)
        {
            return Math.Abs(valueRight.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartAbs();
        }
    }
}
