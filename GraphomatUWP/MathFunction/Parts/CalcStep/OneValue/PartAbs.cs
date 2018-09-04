using System;

namespace MathFunction
{
    class PartAbs : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "abs" };
        }

        protected override double Calc()
        {
            return Math.Abs(Value2.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartAbs();
        }
    }
}
