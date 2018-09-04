using System;

namespace MathFunction
{
    class PartLn : PartResultOneValue
    {
        protected override string[] GetLowerLooks()
        {
            return new string[] { "ln" };
        }

        public override double GetResult(double x)
        {
            return Math.Log(valueRight.GetResult(x));
        }

        public override Part Clone()
        {
            return new PartLn();
        }
    }
}
