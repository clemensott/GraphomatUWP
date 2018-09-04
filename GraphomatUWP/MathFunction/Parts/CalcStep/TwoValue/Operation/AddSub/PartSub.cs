using System.Collections.Generic;

namespace MathFunction
{
    class PartSub : PartAddSub
    {
        public override void ChangeToPartSignIfSub(ref FunctionParts parts)
        {
            parts[parts.IndexOf(this)] = new PartSign();
        }

        protected override double Calc()
        {
            return Value1.Value - Value2.Value;
        }

        public override string ToEquationString()
        {
            return " - ";
        }

        public override PartCalc Clone()
        {
            return new PartSub();
        }
    }
}
