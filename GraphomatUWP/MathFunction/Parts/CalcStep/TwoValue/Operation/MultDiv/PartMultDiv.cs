namespace MathFunction
{
    abstract class PartMultDiv : PartOperation
    {
        public override int GetKindPriority()
        {
            return 6;
        }

        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.MultDiv;
        }

        protected override bool IsOptimalNextPart(PartRuleKind nextKind)
        {
            return nextKind == PartRuleKind.OpenBracket ||
                nextKind == PartRuleKind.OneValueFunction || nextKind == PartRuleKind.Value;
        }

        protected override bool IsPossibleNextPart(PartRuleKind nextKind)
        {
            return nextKind == PartRuleKind.AddSub || IsOptimalNextPart(nextKind);
        }
    }
}
