using System.Collections.Generic;

namespace MathFunction
{
    abstract class PartAddSub : PartOperation
    {
        public override int GetKindPriority()
        {
            return 7;
        }

        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.AddSub;
        }

        protected override bool IsOptimalNextPart(PartRuleKind nextKind)
        {
            return nextKind == PartRuleKind.OpenBracketStrocke ||
                nextKind == PartRuleKind.OneValueFunction || nextKind == PartRuleKind.Value;
        }

        public abstract void ChangeToPartSignIfSub(ref FunctionParts parts);
    }
}
