using System;

namespace MathFunction
{
    abstract class PartCalcOneValue : PartCalc
    {
        public override int GetKindPriority()
        {
            return 3;
        }

        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.OneValueFunction;
        }

        public override void Do()
        {
            Value2.Value = Calc();
        }
    }
}
