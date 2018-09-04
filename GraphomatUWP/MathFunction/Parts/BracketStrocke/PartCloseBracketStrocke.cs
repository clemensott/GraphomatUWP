using System;
using System.Collections.Generic;

namespace MathFunction
{
    abstract class PartCloseBracketStrocke : PartBracketStrocke
    {
        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.CloseBracketStrocke;
        }

        protected override bool WasAbleToChangeIfNecessary(ref FunctionParts parts, int thisIndex, PartRuleKind nextKind)
        {
            if (nextKind == PartRuleKind.OpenBracketStrocke ||
                 nextKind == PartRuleKind.OneValueFunction || nextKind == PartRuleKind.Value)
            {
                parts.Insert(thisIndex + 1, new PartMult());
                return true;
            }

            return false;
        }

        protected override bool IsOptimalNextPart(PartRuleKind nextKind)
        {
            return nextKind == PartRuleKind.AddSub || nextKind == PartRuleKind.CloseBracketStrocke ||
                nextKind == PartRuleKind.MultDiv;
        }

        protected override bool IsPossibleNextPart(PartRuleKind nextKind)
        {
            if (IsOptimalNextPart(nextKind)) return true;

            return nextKind == PartRuleKind.OpenBracketStrocke ||
                nextKind == PartRuleKind.OneValueFunction || nextKind == PartRuleKind.Value;
        }

        public override int ChangeCurrentLevel()
        {
            return -1;
        }
    }
}
