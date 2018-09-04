using System;

namespace MathFunction
{
    class PartAsin : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "asin", "arcsin" };
        }

        protected override double Calc()
        {
            return Math.Asin(Value2.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartAsin();
        }
    }
}
