namespace MathFunction
{
    abstract class PartMultDiv : PartOperation
    {
        public override int GetPriorityValue()
        {
            return 4;
        }

        public override PartRuleType GetRuleType()
        {
            return PartRuleType.MultDiv;
        }

        protected override bool IsPossibleNextPart(PartRuleType nextType)
        {
            return nextType == PartRuleType.AddSub || IsOptimalNextPart(nextType);
        }
    }
}
