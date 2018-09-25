using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    class PartEnd : Part
    {
        public override PartRuleType GetRuleType()
        {
            return PartRuleType.End;
        }

        protected override bool WasAbleToChangeIfNecessary(Parts parts, int thisIndex, PartRuleType nextType)
        {
            if (nextType == PartRuleType.Start ||
                 nextType == PartRuleType.OneValueFunction || nextType == PartRuleType.Value)
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

            return nextType == PartRuleType.Start ||
                nextType == PartRuleType.OneValueFunction || nextType == PartRuleType.Value;
        }

        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return ")";
            yield return "]";
            yield return "}";
        }
    }
}
