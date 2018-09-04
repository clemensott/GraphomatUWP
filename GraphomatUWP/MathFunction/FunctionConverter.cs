using System;
using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    class FunctionConverter
    {
        private const string numberReplaceChar = "!";
        private string equation;
        private FunctionParts parts;

        private FunctionConverter(string equation)
        {
            this.equation = equation;
        }

        public static FunctionParts GetCalcParts(string equation)
        {
            FunctionConverter converter = new FunctionConverter(equation);

            converter.ImprovedEquationSetCalcParts();

            return converter.parts;
        }

        private void ImprovedEquationSetCalcParts()
        {
            if (string.IsNullOrEmpty(equation)) throw new ArgumentException("Equation is null or empty.");

            equation = "(" + equation.Replace(" ", "").ToLower() + ")";

            parts = GetAsFunctionPartList();

            CheckIfPossibleEquationAndImprove();
            ImproveFunctionParts();
        }

        private FunctionParts GetAsFunctionPartList()
        {
            parts = new FunctionParts(equation.Length);

            AddValuesParts();
            AddOtherParts();

            AddMissingBrackets();
            RemoveNullFromParts();

            return parts;
        }

        private void AddValuesParts()
        {
            string equationWithReplacedNumbers = "";

            for (int i = 0; i < equation.Length; i++)
            {
                if (parts[i] == null && IsDoubleChar(equation[i]))
                {
                    int startIndex = i;
                    string valueText = "";

                    do
                    {
                        valueText += equation[i].ToString();
                        equationWithReplacedNumbers += numberReplaceChar;
                        i++;
                    }
                    while (i < equation.Length && IsDoubleChar(equation[i]));

                    equationWithReplacedNumbers += equation[i];

                    parts[startIndex] = new PartValue(valueText);
                }
                else equationWithReplacedNumbers += equation[i].ToString();
            }

            equation = equationWithReplacedNumbers;
        }

        private bool IsDoubleChar(char c)
        {
            return char.IsNumber(c) || c == ',' || c == '.' || c == 'e';
        }

        private void AddOtherParts()
        {
            for (int i = 0; i < equation.Length; i++)
            {
                foreach (FunctionPart partType in FunctionParts.AllTypes)
                {
                    if (partType.IsType(equation, ref i))
                    {
                        if(partType is PartSign) { }
                        i--;
                        parts[i] = partType.Clone();
                        break;
                    }
                }
            }
        }

        private void AddMissingBrackets()
        {
            int level = 0;

            foreach (char c in equation)
            {
                if (c == '(') level++;
                else if (c == ')') level--;

                if (level < 0)
                {
                    parts.Insert(0, new PartOpenBracket());
                    level++;
                }
            }

            while (level > 0)
            {
                parts.Insert(0, new PartCloseBracket());
                level--;
            }
        }

        private void RemoveNullFromParts()
        {
            for (int i = parts.Count - 1; i >= 0; i--)
            {
                if (parts[i] == null) parts.RemoveAt(i);
            }
        }

        private void CheckIfPossibleEquationAndImprove()
        {
            if (parts.Count == 0) throw new ArgumentException("No possible argument.");

            CheckIfEquationIsPossible();
        }

        private void CheckIfEquationIsPossible()
        {
            for (int i = 0; i < parts.Count - 1; i++)
            {
                parts[i].CheckIfPossibleNextPart(ref parts);
            }
        }

        private void ImproveFunctionParts()
        {
            int partsCountBeforeImpoving;

            do
            {
                partsCountBeforeImpoving = parts.Count;

                RemoveUnnecessaryBrackets();
                //ResetValueVariableDependening();
            }
            while (parts.Count != partsCountBeforeImpoving);

            SetValuesAndLevels();

            parts.Remove(parts.First());
            parts.Remove(parts.Last());
        }

        private void RemoveUnnecessaryBrackets()
        {
            RemoveUnnecessaryBracketsNextLevel(0);
        }

        private int RemoveUnnecessaryBracketsNextLevel(int levelStartIndex)
        {
            int i = levelStartIndex + 1;
            bool beforeUnnecessaryPossible = IsPartAddOrOpenBracket(levelStartIndex - 1);
            bool haveCalcStep = false;

            while (i < parts.Count)
            {
                if (parts[i].GetRuleKind() == PartRuleKind.OpenBracket)
                {
                    i = RemoveUnnecessaryBracketsNextLevel(i);
                }
                else if (parts[i].GetActionKind() == PartActionKind.CalcStep) haveCalcStep = true;
                else if (parts[i].GetRuleKind() == PartRuleKind.CloseBracket) break;

                i++;
            }

            return RemoveBracketsIfPossible(levelStartIndex, i, haveCalcStep, beforeUnnecessaryPossible);
        }

        private bool IsPartAddOrOpenBracket(int index)
        {
            if (index < 0) return true;
            if (parts[index].GetRuleKind() == PartRuleKind.OpenBracket) return true;
            if (parts[index].GetRuleKind() != PartRuleKind.AddSub) return false;

            return parts[index] is PartAdd;
        }

        private bool IsPartAddSubOrCloseBracket(int index)
        {
            return index >= parts.Count || parts[index].GetRuleKind() == PartRuleKind.AddSub ||
                parts[index].GetRuleKind() == PartRuleKind.CloseBracket;
        }

        private int RemoveBracketsIfPossible(int levelStartIndex, int curIndex,
            bool haveCalcStep, bool beforeUnnecessaryPossible)
        {
            if (!IsBracketsRemove(levelStartIndex,
                curIndex, haveCalcStep, beforeUnnecessaryPossible)) return curIndex;

            parts.RemoveAt(curIndex);
            curIndex--;
            parts.RemoveAt(levelStartIndex);
            curIndex--;

            return curIndex;
        }

        private bool IsBracketsRemove(int levelStartIndex, int curIndex,
            bool haveCalcStep, bool beforeUnnecessaryPossible)
        {
            if (levelStartIndex == 0 || curIndex == parts.Count - 1) return false;

            if (IsBracket(levelStartIndex))
            {
                return !haveCalcStep || (beforeUnnecessaryPossible && IsPartAddSubOrCloseBracket(curIndex + 1));
            }

            return parts[levelStartIndex + 1].GetRuleKind() == PartRuleKind.OpenBracket &&
                parts[curIndex - 1].GetRuleKind() == PartRuleKind.CloseBracket &&
                !IsBracket(levelStartIndex + 1) && !IsBracket(curIndex - 1);
        }

        private bool IsBracket(int index)
        {
            if (parts[index].GetRuleKind() == PartRuleKind.OpenBracket) return parts[index] is PartOpenBracket;
            else if (parts[index].GetRuleKind() == PartRuleKind.CloseBracket) return parts[index] is PartCloseBracket;

            return false;
        }

        private void SetValuesAndLevels()
        {
            int nextLevelId = 0;
            List<int> levelIds = new List<int>() { nextLevelId };

            for (int i = 0; i < parts.Count; i++)
            {
                if (parts[i].GetActionKind() == PartActionKind.CalcStep)
                {
                    PartValue value1, value2;
                    PartCalc partCalc = parts[i] as PartCalc;

                    GetNextAndPreviousValue(i, out value1, out value2);

                    partCalc.Value1 = value1;
                    partCalc.Value2 = value2;
                    partCalc.Level = GetLevel(levelIds);
                }

                if (parts[i].GetRuleKind() == PartRuleKind.CloseBracket) levelIds.RemoveAt(0);
                else if (parts[i].GetRuleKind() == PartRuleKind.OpenBracket) levelIds.Add(nextLevelId++);
            }
        }

        private void GetNextAndPreviousValue(int index, out PartValue value1, out PartValue value2)
        {
            FunctionPart nextValue = parts.Next(index, PartActionKind.Value);
            FunctionPart previousValue = parts.Previous(index, PartActionKind.Value);

            value2 = nextValue as PartValue;
            value1 = previousValue as PartValue;
        }

        private Level GetLevel(List<int> levelIds)
        {
            int level = -1 * (levelIds.Count - 1);
            int levelId = levelIds.Last();

            return new Level(level, levelId);
        }

        private void ResetValueVariableDependening()
        {
            foreach (FunctionPart part in parts)
            {
                if (part.GetActionKind() == PartActionKind.CalcStep)
                {
                    (part as PartCalc).SetValuesVariableDepending(false);
                }
            }
        }
    }
}
