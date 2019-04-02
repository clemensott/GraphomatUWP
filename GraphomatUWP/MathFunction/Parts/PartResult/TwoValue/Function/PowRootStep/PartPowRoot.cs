namespace MathFunction
{
    abstract class PartPowRoot : PartResultTwoValue
    {
        public override int GetPriorityValue()
        {
            return 6;
        }
    }
}
