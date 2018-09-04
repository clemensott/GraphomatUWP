namespace MathFunction
{
    abstract class PartMultDiv : PartCalcTwoValue
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
            return nextKind == PartRuleKind.AddSub || nextKind == PartRuleKind.OpenBracketStrocke ||
                nextKind == PartRuleKind.OneValueFunction || nextKind == PartRuleKind.Value;
        }

        protected override bool IsPossibleNextPart(PartRuleKind nextKind)
        {
            return IsOptimalNextPart(nextKind);
        }
    }
}
