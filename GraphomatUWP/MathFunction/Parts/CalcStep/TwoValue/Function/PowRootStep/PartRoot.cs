using System;

namespace MathFunction
{
    class PartRoot : PartPowRoot
    {
        protected override double Calc()
        {
            return Math.Pow(Value2.Value, 1 / Value1.Value);
        }

        public override string ToEquationString()
        {
            return "r";
        }

        public override PartCalc Clone()
        {
            return new PartRoot();
        }
    }
}
