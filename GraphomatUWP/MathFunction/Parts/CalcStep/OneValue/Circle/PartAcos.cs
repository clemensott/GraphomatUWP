using System;

namespace MathFunction
{
    class PartAcos : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "acos", "arccos" };
        }

        protected override double Calc()
        {
            return Math.Acos(Value2.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartAcos();
        }
    }
}
