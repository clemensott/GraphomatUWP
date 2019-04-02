namespace MathFunction
{
    abstract class PartResult : Part, IResult
    {
        protected IResult valueRight;

        public double this[double x] => GetResult(x);

        public abstract double GetResult(double x);

        public bool Used { get; set; } = false;

        public PartResultPriority GetPriority()
        {
            return new PartResultPriority(GetPriorityValue(), GetPriorityType());
        }

        public abstract int GetPriorityValue();

        public double GetRelativePriority(RelativeTo relativeTo)
        {
            int priority = GetPriorityValue();

            switch (GetPriorityType())
            {
                case PriorityType.Same:
                    return priority;

                case PriorityType.LeftHigher:
                    return priority + (relativeTo == RelativeTo.Left ? 0.1 : -0.1);

                case PriorityType.RightHigher:
                    return priority + (relativeTo == RelativeTo.Right ? 0.1 : -0.1);
            }

            return priority;
        }

        public abstract PriorityType GetPriorityType();

        protected override bool WasAbleToChangeIfNecessary(Parts parts, int thisIndex, PartRuleType nextType)
        {
            if (nextType == PartRuleType.AddSub)
            {
                ((PartAddSub) parts[thisIndex + 1]).ChangeToPartSignIfSub(parts);

                return true;
            }

            return false;
        }

        protected override bool IsOptimalNextPart(PartRuleType nextType)
        {
            return nextType == PartRuleType.OneValueFunction ||
                nextType == PartRuleType.Start || nextType == PartRuleType.Value;
        }

        protected override bool IsPossibleNextPart(PartRuleType nextType)
        {
            if (IsOptimalNextPart(nextType)) return true;

            return nextType == PartRuleType.AddSub || nextType == PartRuleType.OneValueFunction;
        }

        public abstract void SetValues(Parts parts);
    }
}
