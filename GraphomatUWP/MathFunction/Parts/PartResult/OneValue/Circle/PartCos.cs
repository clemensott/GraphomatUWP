using System;

namespace MathFunction
{
    class PartCos : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "cos" };
        }

        public override double GetResult(double x)
        {
            return Math.Cos(valueRight.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartCos();
        }
    }
}
