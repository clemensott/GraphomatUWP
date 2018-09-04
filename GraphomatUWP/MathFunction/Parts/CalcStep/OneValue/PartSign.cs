namespace MathFunction
{
    class PartSign : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "-" };
        }

        protected override double Calc()
        {
            return -1 * Value2.Value;
        }

        public override FunctionPart Clone()
        {
            return new PartSign();
        }
    }
}
