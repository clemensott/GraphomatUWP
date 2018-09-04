using System;

namespace MathFunction
{
    class PartAcos : PartCalcOneValue
    {
        protected override double Calc()
        {
            return Math.Acos(Value2.Value) * 180.0 / Math.PI;
        }

        public override string ToEquationString()
        {
            return "acos";
        }

        public override PartCalc Clone()
        {
            return new PartAcos();
        }
    }
}
