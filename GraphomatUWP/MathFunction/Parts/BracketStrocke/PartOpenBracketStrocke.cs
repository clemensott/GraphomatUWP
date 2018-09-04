using System.Collections.Generic;

namespace MathFunction
{
    abstract class PartOpenBracketStrocke : PartBracketStrocke
    {
        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.OpenBracketStrocke;
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
                nextKind == PartRuleKind.OpenBracketStrocke || nextKind == PartRuleKind.Value;
        }

        protected override bool IsPossibleNextPart(PartRuleKind nextKind)
        {
            if (IsOptimalNextPart(nextKind)) return true;

            return nextKind == PartRuleKind.AddSub;
        }

        public abstract void AddPartAbsToParts(ref FunctionParts parts);

        public override int ChangeCurrentLevel()
        {
            return 1;
        }
    }
}
