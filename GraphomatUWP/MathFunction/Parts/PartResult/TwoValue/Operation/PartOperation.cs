namespace MathFunction
{
    abstract class PartOperation : PartResultTwoValue
    {
        protected override bool IsOptimalNextPart(PartRuleType nextType)
        {
            return nextType == PartRuleType.Start ||
                nextType == PartRuleType.OneValueFunction || nextType == PartRuleType.Value;
        }

        public override string ToEquationString()
        {
            return " " + base.ToEquationString() + " ";
        }
    }
}
