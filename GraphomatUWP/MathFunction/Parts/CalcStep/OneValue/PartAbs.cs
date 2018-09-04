using System;

namespace MathFunction
{
    class PartAbs : PartCalcOneValue
    {
        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.OpenBracketStrocke;
        }

        protected override double Calc()
        {
            return Math.Abs(Value2.Value);
        }

        public override string ToEquationString()
        {
            return "|";
        }

        public override PartCalc Clone()
        {
            return new PartAbs();
        }
    }
}
