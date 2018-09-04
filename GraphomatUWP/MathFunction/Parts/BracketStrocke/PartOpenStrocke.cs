using System;
using System.Collections.Generic;

namespace MathFunction
{
    class PartOpenStrocke : PartOpenBracketStrocke
    {
        public override void AddPartAbsToParts(ref FunctionParts parts)
        {
            parts[parts.IndexOf(this)] = new PartAbs();
        }

        public override string ToEquationString()
        {
            return "|";
        }
    }
}
