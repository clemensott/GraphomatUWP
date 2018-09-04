using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            (nextPart as PartAddSub).ChangeToPartSignIfSub(parts);

            return true;
        }

        public override PartRuleType GetRuleType()
        {
            return PartRuleType.Start;
        }

        public override Part Clone()
        {
            return new PartStart();
        }
    }
}
