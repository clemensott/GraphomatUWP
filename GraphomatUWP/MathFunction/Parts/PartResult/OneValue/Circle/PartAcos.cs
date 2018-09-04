using System;

namespace MathFunction
{
    class PartAcos : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "acos", "arccos" };
        }

        public override double GetResult(double x)
        {
            return Math.Acos(valueRight.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartAcos();
        }
    }
}
