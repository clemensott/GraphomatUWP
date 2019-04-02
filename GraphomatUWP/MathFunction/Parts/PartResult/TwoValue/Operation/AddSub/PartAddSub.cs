namespace MathFunction
{
    abstract class PartAddSub : PartOperation
    {
        public override int GetPriorityValue()
        {
            return 3;
        }

        public override PartRuleType GetRuleType()
        {
            return PartRuleType.AddSub;
        }

        public abstract void ChangeToPartSignIfSub(Parts parts);
    }
}
