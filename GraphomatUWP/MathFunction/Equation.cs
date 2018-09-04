using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    class Equation : List<char>
    {
        public static Equation GetBasicImprovedEquation(string equation)
        {
            int level = 0;
            string improvedEquation = "";

            foreach (char c in equation)
            {
                if (c != ' ')
                {
                    if (c == '(') level++;
                    else if (c == ')') level--;

                    if (level < 0)
                    {
                        level++;
                        improvedEquation = '(' + improvedEquation;
                    }

                    improvedEquation += c;
                }
            }

            while (level > 0)
            {
                improvedEquation += ")";
                level--;
            }

            if (improvedEquation == "") throw new ArgumentException("Equation is null or empty.");

            return new Equation(improvedEquation);
        }

        public static Equation GetShorted(Equation equation)
        {
            int level = 1;
            string higherLevelEquation = "";

            equation.RemoveAt(0);

            while (equation.Count > 0)
            {
                char c = equation[0];
                equation.RemoveAt(0);

                if (c == '(') level++;
                else if (c == ')') level--;

                if (level == 0) return new Equation(higherLevelEquation);

                higherLevelEquation += c;
            }

            return new Equation("");
        }

        private Equation(string equation) : base(equation)
        {
        }
    }
}
