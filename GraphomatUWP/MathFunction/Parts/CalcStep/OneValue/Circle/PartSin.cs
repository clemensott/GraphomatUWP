using System;

namespace MathFunction
{
    class PartSin : PartCalcOneValue
    {
        protected override double Calc()
        {
            return Math.Sin(Value2.Value / 180.0 * Math.PI);
        }

        public override string ToEquationString()
        {
            return "sin";
        }

        public override PartCalc Clone()
        {
            return new PartSin();
        }
    }
}
