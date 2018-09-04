using System;

namespace MathFunction
{
    class PartSimpleValue : PartValue
    {
        public double Value { get; protected set; }

        public PartSimpleValue(string value) : this(double.Parse(value))
        {
        }

        public PartSimpleValue(double value)
        {
            Value = value;
        }

        public override double GetResult(double x)
        {
            return Value;
        }

        protected override string[] GetLowerLooks()
        {
            return new string[] { Value.ToString() };
        }

        public override string ToString()
        {
            return ToEquationString();
        }

        public override Part Clone()
        {
            return new PartSimpleValue(Value);
        }
    }
}
