using System;

namespace MathFunction
{
    class PartLg : PartCalcOneValue
    {
        protected override double Calc()
        {
            return Math.Log10(Value2.Value);
        }

        public override string ToEquationString()
        {
            return "lg";
        }

        public override PartCalc Clone()
        {
            return new PartLg();
        }
    }
}
