using System;

namespace MathFunction
{
    class PartAsin : PartCalcOneValue
    {
        protected override double Calc()
        {
            return Math.Asin(Value2.Value) * 180.0 / Math.PI;
        }

        public override string ToEquationString()
        {
            return "asin";
        }

        public override PartCalc Clone()
        {
            return new PartAsin();
        }
    }
}
