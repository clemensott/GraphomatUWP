namespace MathFunction
{
    struct PartResultPriority
    {
        public int TypePriority { get; }

        public PriorityType PositionType { get; }

        public PartResultPriority(int priority, PriorityType type)
        {
            TypePriority = priority;
            PositionType = type;
        }

        public int GetPositionPriority(int index, int count)
        {
            if (PositionType == PriorityType.Same) return count;

            return PositionType == PriorityType.RightHigher ? index : (count - index);
        }
    }
}
