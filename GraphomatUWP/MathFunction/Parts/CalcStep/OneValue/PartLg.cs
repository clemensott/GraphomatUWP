using System;

namespace MathFunction
{
    class PartLg : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "lg" };
        }

        protected override double Calc()
        {
            return Math.Log10(Value2.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartLg();
        }
    }
}
