using System;

namespace MathFunction
{
    abstract class PartResultOneValue : PartResult
    {
        public override int GetPriorityValue()
        {
            return 7;
        }

        public override PriorityType GetPriorityType()
        {
            return PriorityType.RightHigher;
        }

        public override PartRuleType GetRuleType()
        {
            return PartRuleType.OneValueFunction;
        }

        public override void SetValues(Parts parts)
        {
            valueRight = parts.RightPartResultWithHigherPriorityNotUsed(this);
            valueRight.Used = true;

            valueRight.SetValues(parts);
        }
    }
}
