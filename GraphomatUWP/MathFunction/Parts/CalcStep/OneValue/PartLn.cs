using System;

namespace MathFunction
{
    class PartLn : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "ln" };
        }

        protected override double Calc()
        {
            return Math.Log(Value2.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartLn();
        }
    }
}
