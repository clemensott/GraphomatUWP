using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartOpenBracket : PartBracket
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "(", "[", "{" };
        }

        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.OpenBracket;
        }

        protected override bool WasAbleToChangeIfNecessary(ref FunctionParts parts, int thisIndex, PartRuleKind nextKind)
        {
            if (nextKind == PartRuleKind.AddSub)
            {
                (parts[thisIndex + 1] as PartAddSub).ChangeToPartSignIfSub(ref parts);
                return true;
            }

            return false;
        }

        protected override bool IsOptimalNextPart(PartRuleKind nextKind)
        {
            return nextKind == PartRuleKind.OneValueFunction ||
                nextKind == PartRuleKind.OpenBracket || nextKind == PartRuleKind.Value;
        }

        protected override bool IsPossibleNextPart(PartRuleKind nextKind)
        {
            if (IsOptimalNextPart(nextKind)) return true;

            return nextKind == PartRuleKind.AddSub;
        }

        public override FunctionPart Clone()
        {
            return new PartOpenBracket();
        }
    }
}
