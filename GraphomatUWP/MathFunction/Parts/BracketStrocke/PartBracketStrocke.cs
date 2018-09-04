namespace MathFunction
{
    abstract class PartBracketStrocke : FunctionPart
    {
        public override PartActionKind GetActionKind()
        {
            return PartActionKind.BracketOrStrocke;
        }

        public abstract int ChangeCurrentLevel();
    }
}
