using System;

namespace MathFunction
{
    class PartSin : PartCalcOneValue
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "sin" };
        }

        protected override double Calc()
        {
            return Math.Sin(Value2.Value);
        }
        
        public override FunctionPart Clone()
        {
            return new PartSin();
        }
    }
}
