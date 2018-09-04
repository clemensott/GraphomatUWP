namespace MathFunction
{
    class PartVariable : PartValue
    {
        public PartVariable() : base(double.NaN)
        {
            IsVariable = IsVariableDepending = true;
        }

        public override string[] GetLowerLooks()
        {
            return new string[] { "x" };
        }

        public override FunctionPart Clone()
        {
            return new PartVariable();
        }

        public override string ToEquationString()
        {
            return GetLowerLooks()[0];
        }

        public override string ToString()
        {
            return base.ToEquationString();
        }
    }
}
