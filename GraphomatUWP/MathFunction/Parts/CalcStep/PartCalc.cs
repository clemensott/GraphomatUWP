using System;
using System.Collections.Generic;

namespace MathFunction
{
    abstract class PartCalc : FunctionPart
    {
        public Level Level { get; set; }

        public PartValue Value1 { get; set; }

        public PartValue Value2 { get; set; }

        public abstract int GetKindPriority();

        public override PartActionKind GetActionKind()
        {
            return PartActionKind.CalcStep;
        }

        protected override bool WasAbleToChangeIfNecessary(ref FunctionParts parts, int thisIndex, PartRuleKind nextKind)
        {
            if (nextKind == PartRuleKind.OneValueFunction)
            {
                parts.Insert(thisIndex - 1, new PartOpenBracket());
                return true;
            }
            else if (nextKind == PartRuleKind.AddSub)
            {
                (parts[thisIndex + 1] as PartAddSub).ChangeToPartSignIfSub(ref parts);
                return true;
            }

            return false;
        }

        protected override bool IsOptimalNextPart(PartRuleKind nextKind)
        {
            return nextKind == PartRuleKind.OpenBracketStrocke || nextKind == PartRuleKind.Value;
        }

        protected override bool IsPossibleNextPart(PartRuleKind nextKind)
        {
            if (IsOptimalNextPart(nextKind)) return true;

            return nextKind == PartRuleKind.AddSub || nextKind == PartRuleKind.OneValueFunction;
        }

        public virtual bool IsChangeOrderAble()
        {
            return false;
        }

        public abstract void Do();

        protected abstract double Calc();

        public void SetVaribaleValue(double value)
        {
            if (Value1.IsVariable) Value1.Value = value;
            if (Value2.IsVariable) Value2.Value = value;
        }

        public int VariableDependingLevel()
        {
            return (Value1.IsVariableDepending ? 1 : 0) + (Value2.IsVariableDepending ? 1 : 0);
        }

        public void SetValuesVariableDepending(bool value)
        {
            if (Value1 != null) Value1.IsVariableDepending = value;

            Value2.IsVariableDepending = value;
        }

        public abstract PartCalc Clone();
    }
}
