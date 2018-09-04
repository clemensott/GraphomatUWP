using System;

namespace MathFunction
{
    class PartVariable : PartValue
    {
        public PartVariable()
        {
        }

        protected override string[] GetLowerLooks()
        {
            return new string[] { "x" };
        }

        public override string ToEquationString()
        {
            return GetLowerLooks()[0];
        }

        public override string ToString()
        {
            return base.ToEquationString();
        }

        public override double GetResult(double x)
        {
            return x;
        }

        public override Part Clone()
        {
            return new PartVariable();
        }
    }
}
