using System;

namespace MathFunction
{
    class PartRoot : PartPowRoot
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "r" };
        }

        protected override double Calc()
        {
            return Math.Pow(Value2.Value, 1 / Value1.Value);
        }

        public override FunctionPart Clone()
        {
            return new PartRoot();
        }
    }
}
