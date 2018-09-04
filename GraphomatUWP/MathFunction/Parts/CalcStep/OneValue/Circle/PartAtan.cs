using System;

namespace MathFunction
{
    class PartAtan : PartCalcOneValue
    {
        protected override double Calc()
        {
            return Math.Atan(Value2.Value) * 180.0 / Math.PI;
        }

        public override string ToEquationString()
        {
            return "atan";
        }

        public override PartCalc Clone()
        {
            return new PartAtan();
        }
    }
}
