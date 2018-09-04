using System.Collections.Generic;

namespace MathFunction
{
    class PartValue : FunctionPart
    {
        private bool isVariableDepending;
        private double value;
        protected PartValue realValue;

        public bool IsVariable { get; protected set; }

        public bool IsVariableDepending
        {
            get { return IsVariable || isVariableDepending; }
            set { isVariableDepending = true; }
        }

        public double Value
        {
            get { return realValue == null ? value : realValue.Value; }
            set
            {
                if (realValue == null) this.value = value;
                else realValue.Value = value;
            }
        }

        public PartValue() : this(double.NaN)
        {
            IsVariable = IsVariableDepending = true;
        }

        public PartValue(string value) : this(double.Parse(value))
        {
        }

        public PartValue(double value)
        {
            this.value = value;
        }

        protected override bool WasAbleToChangeIfNecessary(ref FunctionParts parts,
            int thisIndex, PartRuleKind nextKind)
        {
            if (nextKind == PartRuleKind.OpenBracketStrocke || nextKind == PartRuleKind.OneValueFunction)
            {
                parts.Insert(thisIndex + 1, new PartMult());
                return true;
            }

            return false;
        }

        protected override bool IsOptimalNextPart(PartRuleKind nextKind)
        {
            return nextKind == PartRuleKind.AddSub || nextKind == PartRuleKind.CloseBracketStrocke ||
                nextKind == PartRuleKind.MultDiv || nextKind == PartRuleKind.TwoValueFunction;
        }

        protected override bool IsPossibleNextPart(PartRuleKind nextKind)
        {
            if (IsOptimalNextPart(nextKind)) return true;

            return nextKind == PartRuleKind.OpenBracketStrocke || nextKind == PartRuleKind.OneValueFunction;
        }

        public override PartRuleKind GetRuleKind()
        {
            return PartRuleKind.Value;
        }

        public override PartActionKind GetActionKind()
        {
            return PartActionKind.Value;
        }

        public static void CombinePartValuesAndSetValue(PartValue value1, PartValue value2, double setValue)
        {
            value2.Value = setValue;
            value1.SetRealValue(value2);
        }

        private void SetRealValue(PartValue value)
        {
            if (realValue == null) realValue = value;
            else realValue.SetRealValue(value);
        }

        public override string ToEquationString()
        {
            return IsVariable ? "x" : Value.ToString();
        }
    }
}
