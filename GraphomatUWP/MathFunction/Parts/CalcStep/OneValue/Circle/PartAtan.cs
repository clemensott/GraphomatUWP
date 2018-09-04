using System;

namespace MathFunction
{
    class PartAtan : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "atan", "arctan" };
        }

        protected override double Calc()
        {
            return Math.Atan(Value2.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartAtan();
        }
    }
}
