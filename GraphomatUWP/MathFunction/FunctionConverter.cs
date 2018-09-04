using System;
using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    class FunctionConverter
    {
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
            equation = "(" + equation.Replace(" ", "").ToLower() + ")";

            parts = GetAsFunctionPartList();
            CheckIfPossibleEquationAndImprove();
            ImproveFunctionParts();
        }

        private FunctionParts GetAsFunctionPartList()
        {
            if (string.IsNullOrEmpty(equation)) throw new ArgumentException("Equation is null or empty.");

            parts = new FunctionParts();

            foreach (char c in equation) parts.Add(null);

            AddSingleCharParts();
            AddMultiCharParts();
            AddValuesParts();

            BracketStrockeCheck.Do(ref parts, equation);

            for (int i = parts.Count - 1; i >= 0; i--)
            {
                if (parts[i] == null) parts.RemoveAt(i);
            }

            return parts;
        }

        private void AddSingleCharParts()
        {
            string shortedEquation = equation;

            for (int i = 0; i < equation.Length; i++)
            {
                switch (equation[i])
                {
                    case '(':
                        parts[i] = new PartOpenBracket();
                        break;

                    case ')':
                        parts[i] = new PartCloseBracket();
                        break;

                    case '+':
                        parts[i] = new PartAdd();
                        break;

                    case '-':
                        parts[i] = new PartSub();
                        break;

                    case '*':
                        parts[i] = new PartMult();
                        break;

                    case '/':
                        parts[i] = new PartDiv();
                        break;

                    case '^':
                        parts[i] = new PartPow();
                        break;

                    case 'r':
                        parts[i] = new PartRoot();
                        break;

                    case 'x':
                        parts[i] = new PartValue();
                        break;

                    case 'e':
                        parts[i] = new PartValue(Math.E);
                        break;
                }
            }
        }

        private void AddMultiCharParts()
        {
            string pi = "pi", asin = "asin", acos = "acos", atan = "atan", sin = "sin",
                 cos = "cos", tan = "tan", log = "log", ln = "ln", lg = "lg";

            for (int i = 0; i < equation.Length; i++)
            {
                string shortedEquation = equation.Remove(0, i);

                if (shortedEquation.StartsWith(pi))
                {
                    parts[i] = new PartValue(Math.PI);
                    i += pi.Length - 1;
                }
                else if (shortedEquation.StartsWith(asin))
                {
                    parts[i] = new PartAsin();
                    i += asin.Length - 1;
                }
                else if (shortedEquation.StartsWith(acos))
                {
                    parts[i] = new PartAcos();
                    i += acos.Length - 1;
                }
                else if (shortedEquation.StartsWith(atan))
                {
                    parts[i] = new PartAtan();
                    i += atan.Length - 1;
                }
                else if (shortedEquation.StartsWith(sin))
                {
                    parts[i] = new PartSin();
                    i += sin.Length - 1;
                }
                else if (shortedEquation.StartsWith(cos))
                {
                    parts[i] = new PartCos();
                    i += cos.Length - 1;
                }
                else if (shortedEquation.StartsWith(tan))
                {
                    parts[i] = new PartTan();
                    i += tan.Length - 1;
                }
                else if (shortedEquation.StartsWith(log))
                {
                    parts[i] = new PartLog();
                    i += log.Length - 1;
                }
                else if (shortedEquation.StartsWith(ln))
                {
                    parts[i] = new PartLn();
                    i += ln.Length - 1;
                }
                else if (shortedEquation.StartsWith(lg))
                {
                    parts[i] = new PartLg();
                    i += lg.Length - 1;
                }
            }
        }

        private void AddValuesParts()
        {
            for (int i = 0; i < equation.Length; i++)
            {
                if (parts[i] == null && IsDoubleChar(equation[i]))
                {
                    int j = 1;

                    while (equation.Length > i + j && IsDoubleChar(equation[i + j])) j++;

                    parts[i] = new PartValue(equation.Remove(0, i).Remove(j));
                    i += j - 1;
                }
            }
        }

        private bool IsDoubleChar(char c)
        {
            return char.IsNumber(c) || c == ',' || c == '.' || c == 'e';
        }

        private void CheckIfPossibleEquationAndImprove()
        {
            if (parts.Count == 0) throw new ArgumentException("No possible argument.");

            RemoveNullFromParts();

            CheckIfEquationIsPossible();
            AddPartAbsToParts();
        }

        private void RemoveNullFromParts()
        {
            for (int i = parts.Count - 1; i >= 0; i--)
            {
                if (parts[i] == null) parts.RemoveAt(i);
            }
        }

        private void CheckIfEquationIsPossible()
        {
            for (int i = 0; i < parts.Count - 1; i++)
            {
                parts[i].CheckIfPossibleNextPart(ref parts);
            }
        }

        private void AddPartAbsToParts()
        {
            for (int i = parts.Count - 1; i >= 0; i--)
            {
                if (parts[i].GetRuleKind() == PartRuleKind.OpenBracketStrocke)
                {
                    (parts[i] as PartOpenBracketStrocke).AddPartAbsToParts(ref parts);
                }
            }
        }

        private void ImproveFunctionParts()
        {
            int partsCountBeforeImpoving;

            while (true)
            {
                partsCountBeforeImpoving = parts.Count;

                RemoveUnnecessaryBracketsAndStrockes();
                SetValuesAndLevels();
                CalculateWhatPossibleIs();

                if (parts.Count == partsCountBeforeImpoving) break;

                ResetValueVariableDependening();
            }

            parts.Remove(parts.First());
            parts.Remove(parts.Last());
        }

        private void RemoveUnnecessaryBracketsAndStrockes()
        {
            RemoveUnnecessaryBracketsAndStrockesNextLevel(0);
        }

        private int RemoveUnnecessaryBracketsAndStrockesNextLevel(int levelStartIndex)
        {
            int i = levelStartIndex + 1;
            bool beforeUnnecessaryPossible = IsPartAddOrOpenBracket(levelStartIndex - 1);
            bool haveCalcStep = false;

            while (i < parts.Count)
            {
                if (parts[i].GetRuleKind() == PartRuleKind.OpenBracketStrocke)
                {
                    i = RemoveUnnecessaryBracketsAndStrockesNextLevel(i);
                }
                else if (parts[i].GetActionKind() == PartActionKind.CalcStep) haveCalcStep = true;
                else if (parts[i].GetRuleKind() == PartRuleKind.CloseBracketStrocke) break;

                i++;
            }

            return RemoveBracketsStrockesIfPossible(levelStartIndex, i, haveCalcStep, beforeUnnecessaryPossible);
        }

        private bool IsPartAddOrOpenBracket(int index)
        {
            if (index < 0) return true;
            if (parts[index].GetRuleKind() == PartRuleKind.OpenBracketStrocke) return true;
            if (parts[index].GetRuleKind() != PartRuleKind.AddSub) return false;

            return parts[index] is PartAdd;
        }

        private bool IsPartAddSubOrCloseBracket(int index)
        {
            return index >= parts.Count || parts[index].GetRuleKind() == PartRuleKind.AddSub ||
                parts[index].GetRuleKind() == PartRuleKind.CloseBracketStrocke;
        }

        private int RemoveBracketsStrockesIfPossible(int levelStartIndex, int currentIndex,
            bool haveCalcStep, bool beforeUnnecessaryPossible)
        {
            if (!IsBracketsStrockesRemove(levelStartIndex,
                currentIndex, haveCalcStep, beforeUnnecessaryPossible)) return currentIndex;

            parts.RemoveAt(currentIndex);
            currentIndex--;
            parts.RemoveAt(levelStartIndex);
            currentIndex--;

            return currentIndex;
        }

        private bool IsBracketsStrockesRemove(int levelStartIndex, int currentIndex,
            bool haveCalcStep, bool beforeUnnecessaryPossible)
        {
            if (levelStartIndex == 0) return false;

            if (IsBracket(parts[levelStartIndex]))
            {
                return !haveCalcStep || (beforeUnnecessaryPossible && IsPartAddSubOrCloseBracket(currentIndex));
            }

            return parts[levelStartIndex + 1].GetRuleKind() == PartRuleKind.OpenBracketStrocke &&
                parts[currentIndex - 1].GetRuleKind() == PartRuleKind.CloseBracketStrocke &&
                !IsBracket(parts[levelStartIndex + 1]) && !IsBracket(parts[currentIndex - 1]);
        }

        private bool IsBracket(FunctionPart part)
        {
            if (part.GetRuleKind() == PartRuleKind.OpenBracketStrocke) return part is PartOpenBracket;
            else if (part.GetRuleKind() == PartRuleKind.CloseBracketStrocke) return part is PartCloseBracket;

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

                if (parts[i].GetRuleKind() == PartRuleKind.CloseBracketStrocke) levelIds.RemoveAt(0);
                else if (parts[i].GetRuleKind() == PartRuleKind.OpenBracketStrocke) levelIds.Add(nextLevelId++);
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

        private void CalculateWhatPossibleIs()
        {
            List<PartCalc> orderedPartCalcs = parts.GetAsOrderedPartCalcs();

            for (int i = 0; i < orderedPartCalcs.Count; i++)
            {
                if (orderedPartCalcs[i].VariableDependingLevel() == 1 && orderedPartCalcs[i].IsChangeOrderAble())
                {
                    for (int j = i + 1; j < orderedPartCalcs.Count; j++)
                    {
                        if (CanChangeOrderOfFunctionParts(orderedPartCalcs[i], orderedPartCalcs[j]))
                        {
                            AdjustValues(j, i, ref orderedPartCalcs);
                            CalculateAndRemovePartCalc(j, ref orderedPartCalcs);
                        }
                    }
                }

                orderedPartCalcs[i].SetValuesVariableDepending(true);

                if (orderedPartCalcs[i].VariableDependingLevel() == 0)
                {
                    orderedPartCalcs[i].Do();
                    parts.Remove(orderedPartCalcs[i]);
                    parts.Remove(orderedPartCalcs[i].Value1);
                }
            }
        }

        private bool CanChangeOrderOfFunctionParts(PartCalc part1, PartCalc part2)
        {
            return part1.GetKindPriority() == part2.GetKindPriority() &&
                part1.Level.Id == part2.Level.Id && part2.VariableDependingLevel() < 2;
        }

        private void AdjustValues(int fromIndex, int toIndex, ref List<PartCalc> orderedPartCalcs)
        {
            ChangeValuesFromPartCalcsAndGetValueF1(orderedPartCalcs[fromIndex] as PartCalc,
                orderedPartCalcs[toIndex] as PartCalc);

            FindOtherPartCalcsWithValueF2AndReplace(fromIndex, orderedPartCalcs);
        }

        private void ChangeValuesFromPartCalcsAndGetValueF1(PartCalc partCalcFrom, PartCalc partCalcTo)
        {
            partCalcFrom.Value1 = partCalcTo.Value1;
            partCalcTo.Value1 = partCalcFrom.Value2;
        }

        private void FindOtherPartCalcsWithValueF2AndReplace(int fromIndex, List<PartCalc> orderedPartCalcs)
        {
            for (int i = fromIndex; i < orderedPartCalcs.Count; i++)
            {
                if (orderedPartCalcs[i].GetActionKind() == PartActionKind.CalcStep)
                {
                    PartCalc partCalcTmp = orderedPartCalcs[i] as PartCalc;

                    if (partCalcTmp.Value1 == orderedPartCalcs[fromIndex].Value2)
                    {
                        partCalcTmp.Value1 = orderedPartCalcs[fromIndex].Value1;
                    }
                }
            }
        }

        private void CalculateAndRemovePartCalc(int index, ref List<PartCalc> orderedCalcParts)
        {
            int value1PartsIndex;

            orderedCalcParts[index].Do();

            value1PartsIndex = parts.IndexOf(orderedCalcParts[index].Value1);

            parts.Remove(orderedCalcParts[index].Value1);
            parts.Remove(orderedCalcParts[index].Value2);
            parts.Remove(orderedCalcParts[index]);

            parts.Insert(value1PartsIndex, orderedCalcParts[index].Value2);

            orderedCalcParts.RemoveAt(index);
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
