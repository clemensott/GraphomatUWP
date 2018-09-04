using System;

namespace MathFunction
{
    abstract class PartResultTwoValue : PartResult
    {
        protected IResult valueLeft;

        public override PartRuleType GetRuleType()
        {
            return PartRuleType.TwoValueFunction;
        }

        public override void SetValues(Parts parts)
        {
            valueRight = parts.RightPartResultWithHigherPriorityNotUsed(this);
            valueLeft = parts.LeftPartResultWithHigherPriorityNotUsed(this);

            valueRight.Used = valueLeft.Used = true;

            valueRight.SetValues(parts);
            valueLeft.SetValues(parts);
        }
    }
}
