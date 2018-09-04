using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    class PartBracket : PartValue
    {
        private static readonly char[] allDoubleChars = new char[] { '0', '1', '2',
            '3', '4', '5', '6', '7', '8', '9', '.', ',', 'e', 'E', '+', '-' };
        private static readonly char[] startDoubleChars = new char[] { '0', '1', '2',
            '3', '4', '5', '6', '7', '8', '9', '.', ',' };

        private Equation equation;
        private Parts parts;

        public PartBracket(string equation) : this(Equation.GetBasicImprovedEquation(equation))
        {
            SetValues(null);
            ImproveParts();
        }

        public PartBracket(Equation equation)
        {
            this.equation = equation;

            SetParts();
        }

        private void SetParts()
        {
            parts = GetAsParts();

            CheckIfEquationIsPossible();
        }

        private Parts GetAsParts()
        {
            parts = new Parts();

            GoThroughEquation();

            return parts;
        }

        private void GoThroughEquation()
        {
            while (equation.Count > 0)
            {
                Part addPart;

                if (IsHeigherBracket(out addPart) || IsSimpleValue(out addPart) ||
                    IsOtherPart(out addPart)) parts.Add(addPart);
                else equation.RemoveAt(0);
            }
        }

        private bool IsHeigherBracket(out Part bracket)
        {
            if (equation[0] != '(')
            {
                bracket = null;
                return false;
            }

            bracket = new PartBracket(Equation.GetShorted(equation));
            return true;
        }

        private bool IsSimpleValue(out Part value)
        {
            value = null;

            if (!startDoubleChars.Contains(equation[0])) return false;

            string simpleValueText = "";
            double preValue = -1, curValue;

            do
            {
                simpleValueText += equation[0];

                if (!double.TryParse(simpleValueText, out curValue))
                {
                    if (preValue == -1) return false;

                    value = new PartSimpleValue(preValue);
                    return true;
                }

                equation.RemoveAt(0);
                preValue = curValue;

            } while (equation.Count > 0 && allDoubleChars.Contains(equation[0]));

            value = new PartSimpleValue(simpleValueText);
            return true;
        }

        private bool IsOtherPart(out Part otherPart)
        {
            foreach (Part partType in Parts.GetAllTypes())
            {
                if (partType.IsType(equation))
                {
                    otherPart = partType;
                    return true;
                }
            }

            otherPart = null;
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
                if (parts[i] is PartBracket)
                {
                    PartBracket bracket = parts[i] as PartBracket;

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
        }

        private double GetRelativePriority(int index, RelativeTo relativeTo)
        {
            IResult iResult = parts[index] as IResult;

            if (iResult == null) return 0;

            return iResult.GetRelativePriority(relativeTo);
        }

        public override Part Clone()
        {
            throw new NotImplementedException();
        }

        public override void SetValues(Parts parts)
        {
            valueRight = this.parts.GetPartResultWithLowestRelativePriority();
            valueRight.Used = true;

            valueRight.SetValues(this.parts);
        }

        public override string ToEquationString()
        {
            string output = "(";

            foreach (Part part in parts)
            {
                output += part.ToEquationString();
            }

            return output + ")";
        }
    }
}
