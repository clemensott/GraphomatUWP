using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartCloseBracket : PartBracket
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { ")", "]", "}" };
        }

        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.CloseBracket;
        }

        protected override bool WasAbleToChangeIfNecessary(ref FunctionParts parts, int thisIndex, PartRuleKind nextKind)
        {
            if (nextKind == PartRuleKind.OpenBracket ||
                 nextKind == PartRuleKind.OneValueFunction || nextKind == PartRuleKind.Value)
            {
                parts.Insert(thisIndex + 1, new PartMult());
                return true;
            }

            return false;
        }

        protected override bool IsOptimalNextPart(PartRuleKind nextKind)
        {
            return nextKind == PartRuleKind.AddSub || nextKind == PartRuleKind.CloseBracket ||
                nextKind == PartRuleKind.MultDiv || nextKind == PartRuleKind.TwoValueFunction;
        }

        protected override bool IsPossibleNextPart(PartRuleKind nextKind)
        {
            if (IsOptimalNextPart(nextKind)) return true;

            return nextKind == PartRuleKind.OpenBracket ||
                nextKind == PartRuleKind.OneValueFunction || nextKind == PartRuleKind.Value;
        }

        public override FunctionPart Clone()
        {
            return new PartCloseBracket();
        }
    }
}
