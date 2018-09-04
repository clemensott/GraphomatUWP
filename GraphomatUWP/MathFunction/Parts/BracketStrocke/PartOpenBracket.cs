using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartOpenBracket : PartOpenBracketStrocke
    {
        public override void AddPartAbsToParts(ref FunctionParts parts)
        {
        }

        public override string ToEquationString()
        {
            return "(";
        }
    }
}
