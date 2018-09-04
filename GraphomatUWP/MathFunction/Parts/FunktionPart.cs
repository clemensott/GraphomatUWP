using System;
using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    enum PartActionKind
    {
        CalcStep, Value, BracketOrStrocke
    }

    enum PartRuleKind
    {
        Value, OpenBracketStrocke, CloseBracketStrocke, OneValueFunction, TwoValueFunction, MultDiv, AddSub
    }

    abstract class FunctionPart
    {
        public abstract PartActionKind GetActionKind();

        public abstract PartRuleKind GetRuleKind();

        public void CheckIfPossibleNextPart(ref FunctionParts parts)
        {
            int thisIndex = parts.IndexOf(this);
            PartRuleKind nextKind = parts[thisIndex + 1].GetRuleKind();

            if (IsOptimalNextPart(nextKind)) return;

            if (!IsPossibleNextPart(nextKind) || !WasAbleToChangeIfNecessary(ref parts, thisIndex, nextKind))
            {
                throw new ArgumentException();
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
                else if (parts[i].GetRuleKind() == PartRuleKind.OpenBracketStrocke) level++;
                else if (parts[i].GetRuleKind() == PartRuleKind.CloseBracketStrocke) level++;

                if (valueReached && level == 0)
                {
                    parts.Insert(openIndex, new PartOpenBracket());
                    parts.Insert(i, new PartCloseBracket());

                    return;
                }
            }

        }

        public abstract string ToEquationString();
    }
}
