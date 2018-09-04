using System;

namespace MathFunction
{
    class PartPow : PartPowRoot
    {
        protected override double Calc()
        {
            return Math.Pow(Value1.Value, Value2.Value);
        }

        public override string ToEquationString()
        {
            return "^";
        }

        public override PartCalc Clone()
        {
            return new PartPow();
        }
    }
}
