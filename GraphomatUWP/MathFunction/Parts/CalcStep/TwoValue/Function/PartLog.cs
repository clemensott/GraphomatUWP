using System;

namespace MathFunction
{
    class PartLog : PartCalcTwoValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "log" };
        }

        public override int GetKindPriority()
        {
            return 5;
        }

        protected override double Calc()
        {
            return Math.Log(Value2.Value, Value1.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartLog();
        }
    }
}
