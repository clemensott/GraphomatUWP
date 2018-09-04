using System;

namespace MathFunction
{
    class PartAtan : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "atan", "arctan" };
        }

        public override double GetResult(double x)
        {
            return Math.Atan(valueRight.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartAtan();
        }
    }
}
