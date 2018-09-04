using System;
using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    class BracketStrockeCheck
    {
        private int level, startIndex, i;
        private string equation;
        private FunctionParts parts;
        private List<int> knownIndexes, unknownIndexes;

        private BracketStrockeCheck(ref FunctionParts functionParts, int currentIndex, int currentLevel)
        {
            level = currentLevel;
            i = currentIndex;
            parts = functionParts;

            knownIndexes = new List<int>();
            unknownIndexes = new List<int>();
        }

        public static void Do(ref FunctionParts functionParts, string equation)
        {
            BracketStrockeCheck check = new BracketStrockeCheck(ref functionParts, 0, 0);

            while (check.Do(ref equation) < functionParts.Count)
            {
                check.AddOpenBracket();

                check.i++;
            }

            while (check.level > 0)
            {
                check.AddCloseBracket();

                check.level--;
            }
        }

        private int Do(ref string equation)
        {
            this.equation = equation;
            startIndex = i;

            while (i < equation.Length)
            {
                if (equation[i] == '|')
                {
                    if (CanIfOpenOrCloseStrocke(ref parts, i)) knownIndexes.Add(i);
                    else unknownIndexes.Add(i);
                }
                else if (equation[i] == '(')
                {
                    BracketStrockeCheck check = new BracketStrockeCheck(ref parts, i + 1, level + 1);
                    i = check.Do(ref equation);
                }
                else if (equation[i] == ')')
                {
                    FindOutIfOpenOrCloseFinally();

                    return i;
                }

                i++;
            }

            FindOutIfOpenOrCloseFinally();

            equation = this.equation;
            return i;
        }

        private bool CanIfOpenOrCloseStrocke(ref FunctionParts parts, int index)
        {
            PartRuleKind nextKind, previousKind = GetPreviousPartRuleKindWhichIsNotIsFollow(parts, index);

            if (previousKind == PartRuleKind.AddSub ||
                previousKind == PartRuleKind.OpenBracketStrocke || previousKind == PartRuleKind.MultDiv)
            {
                parts[index] = new PartOpenStrocke();
                return true;
            }
            else if (previousKind == PartRuleKind.OneValueFunction || previousKind == PartRuleKind.TwoValueFunction)
            {
                throw new ArgumentException(string.Format("Strocke at Index {0} is not possible.", index));
            }

            nextKind = GetNextPartRuleKindWhichIsNotIsFollow(parts, index);

            if (nextKind == PartRuleKind.CloseBracketStrocke || nextKind == PartRuleKind.MultDiv)
            {
                parts[index] = new PartCloseStrocke();
                return true;
            }
            else if (nextKind == PartRuleKind.MultDiv || nextKind == PartRuleKind.TwoValueFunction)
            {
                throw new ArgumentException(string.Format("Strocke at Index {0} is not possible.", index));
            }

            return false;
        }

        private PartRuleKind GetPreviousPartRuleKindWhichIsNotIsFollow(FunctionParts parts, int index)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                if (parts[i] != null) return parts[i].GetRuleKind();
            }

            throw new ArgumentException();
        }

        private PartRuleKind GetNextPartRuleKindWhichIsNotIsFollow(FunctionParts parts, int index)
        {
            for (int i = index + 1; i < parts.Count; i++)
            {
                if (parts[i] != null) return parts[i].GetRuleKind();
            }

            throw new ArgumentException();
        }

        private void FindOutIfOpenOrCloseFinally()
        {
            AddOrSetMissingStrockes();

            if (unknownIndexes.Count > 0) ThrowArgumentExceptionBecauseOfUnkownStrockes();
        }

        private void AddOrSetMissingStrockes()
        {
            int strockeLevel = 0;

            foreach (int index in knownIndexes)
            {
                if (parts[index].GetRuleKind() == PartRuleKind.OpenBracketStrocke) strockeLevel++;
                else if (parts[index].GetRuleKind() == PartRuleKind.CloseBracketStrocke) strockeLevel--;

                if (strockeLevel < 0)
                {
                    if (HaveUnkownStrockeBefore(index)) SetUnkownStrocke(unknownIndexes[0], new PartOpenStrocke());
                    else AddOpenStrocke();

                    strockeLevel++;
                }
            }

            while (strockeLevel > 0)
            {
                if (HaveUnkownStrockeAfter(knownIndexes.Last()))
                {
                    SetUnkownStrocke(unknownIndexes.Last(), new PartOpenStrocke());
                }
                else AddCloseStrocke();

                strockeLevel--;
            }
        }

        private bool HaveUnkownStrockeBefore(int index)
        {
            if (unknownIndexes.Count == 0) return false;

            return unknownIndexes[0] < index;
        }

        private bool HaveUnkownStrockeAfter(int index)
        {
            if (unknownIndexes.Count == 0) return false;

            return unknownIndexes.Last() > index;
        }

        private void SetUnkownStrocke(int index, PartBracketStrocke part)
        {
            int indexOfIndex = unknownIndexes.IndexOf(index);
            parts[index] = part;

            unknownIndexes.RemoveAt(indexOfIndex);
        }

        private void AddOpenBracket()
        {
            AddStrocke(startIndex, '(', new PartOpenBracket());
        }

        private void AddOpenStrocke()
        {
            AddStrocke(startIndex, '|', new PartOpenStrocke());
        }

        private void AddCloseBracket()
        {
            AddStrocke(i, ')', new PartCloseBracket());
        }

        private void AddCloseStrocke()
        {
            AddStrocke(i, '|', new PartCloseStrocke());
        }

        private void AddStrocke(int index, char c, PartBracketStrocke part)
        {
            equation = equation.Remove(index) + c.ToString() + equation.Remove(0, i);

            parts.Add(parts.Last());

            for (int i = parts.Count - 2; i > index; i--) parts[i] = parts[i - 1];

            parts[index] = part;
        }

        private void ThrowArgumentExceptionBecauseOfUnkownStrockes()
        {
            string message = "Unkown state of Strocke(s):";

            foreach (int index in unknownIndexes) message += " " + i.ToString() + ",";

            message = message.TrimEnd(',');

            throw new ArgumentException(message);
        }
    }
}
