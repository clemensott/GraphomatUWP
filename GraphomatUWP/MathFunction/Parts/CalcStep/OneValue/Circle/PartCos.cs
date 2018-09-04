using System;

namespace MathFunction
{
    class PartCos : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "cos" };
        }

        protected override double Calc()
        {
            return Math.Cos(Value2.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartCos();
        }
    }
}
