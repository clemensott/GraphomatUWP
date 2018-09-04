using System;

namespace MathFunction
{
    class PartSign : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "-" };
        }

        public override double GetResult(double x)
        {
            return -1 * valueRight.GetResult(x);
        }

        public override Part Clone()
        {
            return new PartSign();
        }
    }
}
