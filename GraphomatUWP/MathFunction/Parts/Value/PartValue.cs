using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    abstract class PartValue : PartResult
    {
        public override int GetPriorityValue()
        {
            return 8;
        }

        public override PriorityType GetPriorityType()
        {
            return PriorityType.Same;
        }

        public override PartRuleType GetRuleType()
        {
            return PartRuleType.Value;
        }

        public override double GetResult(double x)
        {
            return valueRight.GetResult(x);
        }

        protected override bool WasAbleToChangeIfNecessary(Parts parts,
            int thisIndex, PartRuleType nextType)
        {
            if (nextType == PartRuleType.Start || nextType == PartRuleType.Value ||
                nextType == PartRuleType.OneValueFunction)
            {
                parts.Insert(thisIndex + 1, new PartMult());
                return true;
            }

            return false;
        }

        protected override bool IsOptimalNextPart(PartRuleType nextType)
        {
            return nextType == PartRuleType.AddSub || nextType == PartRuleType.End ||
                nextType == PartRuleType.MultDiv || nextType == PartRuleType.TwoValueFunction;
        }

        protected override bool IsPossibleNextPart(PartRuleType nextType)
        {
            if (IsOptimalNextPart(nextType)) return true;

            return nextType == PartRuleType.Start || nextType == PartRuleType.OneValueFunction ||
                nextType == PartRuleType.Value;
        }

        public override void SetValues(Parts parts)
        {
            valueRight = this;
            Used = true;
        }
    }
}
