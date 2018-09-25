using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    class PartSimpleValue : PartValue
    {
        private static readonly char[] allDoubleChars = new char[] { '0', '1', '2',
            '3', '4', '5', '6', '7', '8', '9', '.', ',', 'e', 'E', '+', '-' };
        private static readonly char[] startDoubleChars = new char[] { '0', '1', '2',
            '3', '4', '5', '6', '7', '8', '9', '.', ',' };

        public double Value { get; protected set; }

        public PartSimpleValue()
        {
        }

        public PartSimpleValue(double value)
        {
            Value = value;
        }

        public PartSimpleValue(string value) : this(double.Parse(value))
        {
        }

        public override double GetResult(double x)
        {
            return Value;
        }

        protected override IEnumerable<string> GetLowerLooks()
        {
            yield return Value.ToString();
        }

        public override bool Matches(Equation equation)
        {
            if (!startDoubleChars.Contains(equation[0])) return false;

            string simpleValueText = "";
            double preValue = -1, curValue;

            do
            {
                simpleValueText += equation[0];

                if (!double.TryParse(simpleValueText, out curValue))
                {
                    if (preValue == -1) return false;

                    Value = preValue;
                    return true;
                }

                equation.RemoveAt(0);
                preValue = curValue;

            } while (equation.Count > 0 && allDoubleChars.Contains(equation[0]));

            Value = curValue;
            return true;
        }

        public override string ToString()
        {
            return ToEquationString();
        }
    }
}
