using System;

namespace MathFunction
{
    class PartLg : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "lg" };
        }

        public override double GetResult(double x)
        {
            return Math.Log10(valueRight.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartLg();
        }
    }
}
