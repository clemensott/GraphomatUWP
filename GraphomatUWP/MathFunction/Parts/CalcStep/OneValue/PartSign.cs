namespace MathFunction
{
    class PartSign : PartCalcOneValue
    {
        protected override double Calc()
        {
            return -1 * Value2.Value;
        }

        public override string ToEquationString()
        {
            return "-";
        }

        public override PartCalc Clone()
        {
            return new PartSign();
        }
    }
}
