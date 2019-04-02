using System.Collections.Generic;

namespace MathFunction
{
    class PartConstants : PartValue
    {
        private string[] names;

        public double Value { get; protected set; }

        public string Name => ToEquationString();

        public PartConstants(double value, params string[] names)
        {
            Value = value;
            this.names = names;
        }

        public PartConstants(string value, params string[] names) : this(double.Parse(value), names)
        {
        }

        public override bool Matches(Equation equation)
        {
            return base.Matches(equation);
        }

        protected override IEnumerable<string> GetLowerLooks()
        {
            return names;
        }

        public override double GetResult(double x)
        {
            return Value;
        }

        public override string ToString()
        {
            return ToEquationString();
        }
    }
}
