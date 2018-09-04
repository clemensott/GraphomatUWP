using System;

namespace MathFunction
{
    class PartTan : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "tan" };
        }

        public override double GetResult(double x)
        {
            return Math.Tan(valueRight.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartTan();
        }
    }
}
