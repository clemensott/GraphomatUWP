using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartAdd : PartAddSub
    {
        public override void ChangeToPartSignIfSub(ref FunctionParts parts)
        {
            parts.Remove(this);
        }

        protected override double Calc()
        {
            return Value1.Value + Value2.Value;
        }

        public override string ToEquationString()
        {
            return " + ";
        }

        public override PartCalc Clone()
        {
            return new PartAdd();
        }
    }
}
