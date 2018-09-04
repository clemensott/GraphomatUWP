using System;

namespace MathFunction
{
    class PartCos : PartCalcOneValue
    {
        protected override double Calc()
        {
            return Math.Cos(Value2.Value / 180.0 * Math.PI);
        }

        public override string ToEquationString()
        {
            return "cos";
        }

        public override PartCalc Clone()
        {
            return new PartCos();
        }
    }
}
