using System;
using System.Collections.Generic;

namespace MathFunction
{
    class Equation : List<char>
    {
        public Equation() : base()
        {
        }

        public Equation(string equation) : base(equation)
        {
        }

        public override string ToString()
        {
            return string.Concat(this);
        }
    }
}
