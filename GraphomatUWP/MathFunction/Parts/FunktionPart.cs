using System;
using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    enum PartActionKind
    {
        CalcStep, Value, Bracket
    }

    enum PartRuleKind
    {
        Value, OpenBracket, CloseBracket, OneValueFunction, TwoValueFunction, MultDiv, AddSub
    }

    abstract class FunctionPart
    {

        public virtual string[] GetLowerLooks()
        {
            return new string[] { "?" };
        }

        public bool IsType(string equation, ref int index)
        {
            int originalIndex = index;

            foreach (string look in GetLowerLooks())
            {
                if (LooksLike(equation, ref index, look)) return true;

                index = originalIndex;
            }

            return false;
        }

        private bool LooksLike(string equation, ref int index, string look)
        {
            for (int i = 0; i < look.Length; i++, index++)
            {
                if (index >= equation.Length || equation[index] != look[i]) return false;
            }

            return true;
        }

        public abstract PartActionKind GetActionKind();

        public abstract PartRuleKind GetRuleKind();

        public void CheckIfPossibleNextPart(ref FunctionParts parts)
        {
            FunctionParts oldParts = new FunctionParts(parts);

            while (true)
            {
                if (parts.Count > 100) { CheckIfPossibleNextPart(ref oldParts); }

                int thisIndex = parts.IndexOf(this);
                PartRuleKind nextKind = parts[thisIndex + 1].GetRuleKind();

                if (IsOptimalNextPart(nextKind)) return;

                if (!IsPossibleNextPart(nextKind) || !WasAbleToChangeIfNecessary(ref parts, thisIndex, nextKind))
                {
                    throw new ArgumentException();
                }
            }
        }

        protected abstract bool IsPossibleNextPart(PartRuleKind nextKind);

        protected abstract bool WasAbleToChangeIfNecessary
            (ref FunctionParts parts, int thisIndex, PartRuleKind nextKind);

        protected abstract bool IsOptimalNextPart(PartRuleKind nextKind);

        protected void AddBrackets(ref FunctionParts parts, int openIndex)
        {
            bool valueReached = false;
            int level = 0;

            for (int i = openIndex; i < parts.Count; i++)
            {
                if (parts[i].GetRuleKind() == PartRuleKind.Value) valueReached = true;
                else if (parts[i].GetRuleKind() == PartRuleKind.OpenBracket) level++;
                else if (parts[i].GetRuleKind() == PartRuleKind.CloseBracket) level++;

                if (valueReached && level == 0)
                {
                    parts.Insert(openIndex, new PartOpenBracket());
                    parts.Insert(i, new PartCloseBracket());

                    return;
                }
            }
        }

        public abstract FunctionPart Clone();

        public virtual string ToEquationString()
        {
            return GetLowerLooks()[0];
        }
    }
}
