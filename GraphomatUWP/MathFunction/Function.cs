using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    public class Function
    {
        private string original, improve;
        private Calculator calculator;

        public double this[double x] { get { return GetResult(x); } }

        public string Equation
        {
            get { return original; }
            set
            {
                if (original == value) return;

                original = value;
                SetImprovedEquation();
            }
        }

        public string ImprovedEquation { get { return improve; } }

        private Function(string equation)
        {
            original = equation;
            SetImprovedEquation();
        }

        public static Function Parse(string s)
        {
            return new Function(s);
        }

        public static bool TryParse(string s, out Function f)
        {
            try
            {
                f = Parse(s);
                return true;
            }
            catch { }

            f = null;
            return false;
        }

        private void SetImprovedEquation()
        {
            FunctionParts parts = FunctionConverter.GetCalcParts(original);

            calculator = new Calculator(parts);
            improve = parts.Equation;
        }

        public double GetResult(double x)
        {
            return calculator.GetResult(x);
        }
    }
}
