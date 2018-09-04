using System;

namespace MathFunction
{
    class PartLn : PartCalcOneValue
    {
        protected override double Calc()
        {
            return Math.Log(Value2.Value);
        }

        public override string ToEquationString()
        {
            return "ln";
        }

        public override PartCalc Clone()
        {
            return new PartLn();
        }
    }
}
