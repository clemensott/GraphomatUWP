using System;

namespace MathFunction
{
    abstract class PartCalcTwoValue : PartCalc
    {
        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.TwoValueFunction;
        }

        public override void Do()
        {
            PartValue.CombinePartValuesAndSetValue(Value1, Value2, Calc());
        }
    }
}
