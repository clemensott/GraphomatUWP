using System;

namespace MathFunction
{
    class PartLog : PartCalcTwoValue
    {
        public override int GetKindPriority()
        {
            return 5;
        }

        protected override double Calc()
        {
            return Math.Log(Value2.Value, Value1.Value);
        }

        public override string ToEquationString()
        {
            return "log";
        }

        public override PartCalc Clone()
        {
            return new PartLog();
        }
    }
}
