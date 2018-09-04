using System;

namespace MathFunction
{
    class PartTan : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "tan" };
        }

        protected override double Calc()
        {
            return Math.Tan(Value2.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartTan();
        }
    }
}
