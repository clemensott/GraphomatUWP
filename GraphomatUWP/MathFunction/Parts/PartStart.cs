using System.Collections.Generic;

namespace MathFunction
{
    class PartStart : Part
    {
        protected override bool IsOptimalNextPart(PartRuleType nextType)
        {
            return nextType == PartRuleType.OneValueFunction ||
                nextType == PartRuleType.Start || nextType == PartRuleType.Value;
        }

        protected override bool IsPossibleNextPart(PartRuleType nextType)
        {
            if (IsOptimalNextPart(nextType)) return true;

            return nextType == PartRuleType.AddSub;
        }

        protected override bool WasAbleToChangeIfNecessary(Parts parts, int thisIndex, PartRuleType nextType)
        {
            Part nextPart = parts[thisIndex + 1];

            if (!(nextPart is PartAddSub)) return false;

            ((PartAddSub) nextPart).ChangeToPartSignIfSub(parts);

            return true;
        }

        public override PartRuleType GetRuleType()
        {
            return PartRuleType.Start;
        }

        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return "(";
            yield return "[";
            yield return "{";
        }
    }
}
