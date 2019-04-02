using System;
using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    class PartBracket : PartValue
    {
        private Equation equation;
        private Parts parts;

        public PartBracket()
        {
        }

        public PartBracket(string equation)
        {
            this.equation = GetBasicImprovedEquation(equation);

            SetParts();
            SetValues(null);
            ImproveParts();
        }

        public PartBracket(Equation equation)
        {
            this.equation = equation;

            SetParts();
        }

        public Equation GetBasicImprovedEquation(string equation)
        {
            int level = 0;
            string improvedEquation = "";

            foreach (char c in equation)
            {
                if (c != ' ')
                {
                    if (IsBeginLook(c)) level++;
                    else if (IsEndLook(c)) level--;

                    if (level < 0)
                    {
                        level++;
                        improvedEquation = GetBeginLooks().First() + improvedEquation;
                    }

                    improvedEquation += c;
                }
            }

            while (level > 0)
            {
                improvedEquation += GetEndLooks().First();
                level--;
            }

            if (improvedEquation == "") throw new ArgumentException("Equation is null or empty.");

            return new Equation(improvedEquation);
        }

        private void SetParts()
        {
            parts = new Parts();

            GoThroughEquation();
            CheckIfEquationIsPossible();
        }

        private void GoThroughEquation()
        {
            while (equation.Count > 0)
            {
                Part addPart;

                if (IsPart(out addPart)) parts.Add(addPart);
                else equation.RemoveAt(0);
            }
        }

        public override bool Matches(Equation equation)
        {
            if (!IsBeginLook(equation[0])) return false;

            this.equation = GetShorted(equation);
            SetParts();

            return true;
        }

        public Equation GetShorted(Equation equation)
        {
            int level = 1;
            string higherLevelEquation = "";

            equation.RemoveAt(0);

            while (equation.Count > 0)
            {
                char c = equation[0];
                equation.RemoveAt(0);

                if (IsBeginLook(c)) level++;
                else if (IsEndLook(c)) level--;

                if (level == 0) return new Equation(higherLevelEquation);

                higherLevelEquation += c;
            }

            return new Equation();
        }

        private bool IsBeginLook(char c)
        {
            return GetBeginLooks().Any(l => l == c);
        }

        private bool IsEndLook(char c)
        {
            return GetEndLooks().Any(l => l == c);
        }

        private IEnumerable<char> GetBeginLooks()
        {
            yield return '(';
            yield return '[';
            yield return '{';
        }

        private IEnumerable<char> GetEndLooks()
        {
            yield return ')';
            yield return ']';
            yield return '}';
        }

        private bool IsPart(out Part part)
        {
            foreach (Part partType in Parts.GetAllTypes())
            {
                if (partType.Matches(equation))
                {
                    part = partType;
                    return true;
                }
            }

            part = null;
            return false;
        }

        private void CheckIfEquationIsPossible()
        {
            if (parts.Count == 0) throw new ArgumentException("No possible argument.");

            for (int i = -1; i < parts.Count; i++)
            {
                parts[i].CheckIfPossibleNextPart(parts);
            }
        }

        private void ImproveParts()
        {
            ImplementBracketsIfPriorityIsHigher();
        }

        private void ImplementBracketsIfPriorityIsHigher()
        {
            for (int i = 0; i < parts.Count; i++)
            {
                if (!(parts[i] is PartBracket)) continue;

                PartBracket bracket = (PartBracket) parts[i];

                bracket.ImplementBracketsIfPriorityIsHigher();

                double relPriBracketToLeft = bracket.valueRight.GetRelativePriority(RelativeTo.Left);
                double relPriBracketToRight = bracket.valueRight.GetRelativePriority(RelativeTo.Right);
                double relPriLeftToRight = GetRelativePriority(i - 1, RelativeTo.Right);
                double relPriRightToLeft = GetRelativePriority(i + 1, RelativeTo.Left);

                if (relPriBracketToLeft > relPriLeftToRight && relPriBracketToRight > relPriRightToLeft)
                {
                    parts.RemoveAt(i);
                    parts.InsertRange(i, bracket.parts);
                }
            }
        }

        private double GetRelativePriority(int index, RelativeTo relativeTo)
        {
            IResult iResult = parts[index] as IResult;

            if (iResult == null) return 0;

            return iResult.GetRelativePriority(relativeTo);
        }

        public override void SetValues(Parts parts)
        {
            valueRight = this.parts.GetPartResultWithLowestRelativePriority();
            valueRight.Used = true;

            valueRight.SetValues(this.parts);
        }

        public override string ToEquationString()
        {
            return "(" + string.Concat(parts) + ")";
        }
    }
}
