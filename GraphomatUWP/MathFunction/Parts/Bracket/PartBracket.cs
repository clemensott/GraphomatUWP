namespace MathFunction
{
    abstract class PartBracket : FunctionPart
    {
        public override PartActionKind GetActionKind()
        {
            return PartActionKind.Bracket;
        }
    }
}
