using System;

namespace MathFunction
{
    class PartTan : PartCalcOneValue
    {
        protected override double Calc()
        {
            return Math.Tan(Value2.Value / 180.0 * Math.PI);
        }

        public override string ToEquationString()
        {
            return "tan";
        }

        public override PartCalc Clone()
        {
            return new PartTan();
        }
    }
}
