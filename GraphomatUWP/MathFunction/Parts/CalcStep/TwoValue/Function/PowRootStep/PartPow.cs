using System;

namespace MathFunction
{
    class PartPow : PartPowRoot
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "^" };
        }

        protected override double Calc()
        {
            return Math.Pow(Value1.Value, Value2.Value);
        }

        public override string ToEquationString()
        {
            return "^";
        }

        public override FunctionPart Clone()
        {
            return new PartPow();
        }
    }
}
