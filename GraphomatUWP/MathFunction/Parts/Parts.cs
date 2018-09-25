using System;
using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    class Parts : List<Part>
    {
        public static IEnumerable<Part> GetAllTypes()
        {
            yield return new PartBracket();
            yield return new PartSimpleValue();
            yield return new PartAcos();
            yield return new PartAsin();
            yield return new PartAtan();
            yield return new PartCos();
            yield return new PartSin();
            yield return new PartTan();
            yield return new PartAbs();
            yield return new PartLg();
            yield return new PartLn();
            yield return new PartPow();
            yield return new PartRoot();
            yield return new PartLog();
            yield return new PartAdd();
            yield return new PartSub();
            yield return new PartDiv();
            yield return new PartMult();
            yield return new PartVariable();
            yield return new PartConstants(Math.PI, "π", "pi");
            yield return new PartConstants(Math.PI / 180.0, "(π ÷ 180)", "(pi / 180)", "°");
            yield return new PartConstants(Math.E, "e");
        }

        new public Part this[int index]
        {
            get
            {
                if (index == -1) return new PartStart();
                if (index == Count) return new PartEnd();

                return base[index];
            }
            set { base[index] = value; }
        }

        public Parts()
        {

        }

        public int ExtendedIndexOf(Part part)
        {
            for (int i = -1; i <= Count; i++) if (this[i] == part) return i;

            return -1;
        }

        public PartResult RightPartResultWithHigherPriorityNotUsed(PartResult curCalc)
        {
            int curIndex = IndexOf(curCalc);
            List<PartResult> possibleParts = new List<PartResult>();

            for (int i = curIndex + 1; i < Count; i++)
            {
                if (this[i] is PartResult)
                {
                    PartResult part = this[i] as PartResult;

                    if (part.Used) break;

                    possibleParts.Add(part);
                }
            }

            return GetPartResultWithLowestRelativePriorityAndNearest(possibleParts, curIndex);
        }

        public PartResult LeftPartResultWithHigherPriorityNotUsed(PartResult curCalc)
        {
            int curIndex = IndexOf(curCalc);
            List<PartResult> possibleParts = new List<PartResult>();

            for (int i = curIndex - 1; i >= 0; i--)
            {
                if (this[i] is PartResult)
                {
                    PartResult part = this[i] as PartResult;

                    if (part.Used) break;

                    possibleParts.Add(part);
                }
            }

            return GetPartResultWithLowestRelativePriorityAndNearest(possibleParts, curIndex);
        }

        public PartResult GetPartResultWithLowestRelativePriority()
        {
            IEnumerable<PartResult> allPartResult = this.OfType<PartResult>();

            return GetPartResultWithLowestRelativePriorityAndNearest(allPartResult, -1);
        }

        public PartResult GetPartResultWithLowestRelativePriorityAndNearest(IEnumerable<PartResult> parts, int toIndex)
        {
            //var list = parts.OrderBy(x => x.GetRelativePriority(GetRelativeTo(x, toIndex))).
            //    ThenBy(x => Math.Abs(IndexOf(x) - toIndex)).ToArray();

            //var order = parts.OrderBy(x => x.GetPriority().TypePriority).ToArray();
            //var list2 = parts.OrderBy(x => x.GetPriority().TypePriority).
            //    ThenBy(x => x.GetPriority().GetPositionPriority(IndexOf(x), Count)).
            //    ThenBy(x => Math.Abs(IndexOf(x) - toIndex)).ToArray();


            //int sum = parts.Sum(x2 => GetRelativePrority(parts.ElementAt(1), x2));
            //var list3s = parts.Select(x1 => parts.Sum(x2 => GetRelativePrority(x1, x2))).ToArray();
            var list3 = parts.OrderBy(x1 => parts.Sum(x2 => GetRelativePrority(x1, x2))).
                ThenBy(x => Math.Abs(IndexOf(x) - toIndex)).ToArray();

            return list3.First();
        }

        private RelativeTo GetRelativeTo(Part partFrom, int toIndex)
        {
            return IndexOf(partFrom) > toIndex ? RelativeTo.Right : RelativeTo.Left;
        }

        private int GetRelativePrority(PartResult part1, PartResult part2)
        {
            int index1 = IndexOf(part1);
            int index2 = IndexOf(part2);

            if (part1 == part2) return 0;
            if (part1.GetPriorityValue() < part2.GetPriorityValue()) return -1;
            if (part1.GetPriorityValue() > part2.GetPriorityValue()) return 1;
            if ((int)part1.GetPriorityType() + (int)part2.GetPriorityType() == 0) return 0;
            if (part1.GetPriorityType() == part2.GetPriorityType()) return (index1 < index2 ? -1 : 1) * (int)part1.GetPriorityType();

            if (index1 < index2)
            {
                if (part1.GetPriorityType() == PriorityType.LeftHigher &&
                    part2.GetPriorityType() == PriorityType.Same) return 1;
                else if (part1.GetPriorityType() == PriorityType.RightHigher &&
                    part2.GetPriorityType() == PriorityType.Same) return 1;
                else if (part1.GetPriorityType() == PriorityType.Same &&
                    part2.GetPriorityType() == PriorityType.LeftHigher) return 0;
                else if (part1.GetPriorityType() == PriorityType.Same &&
                    part2.GetPriorityType() == PriorityType.RightHigher) return 0;
            }

            if (part1.GetPriorityType() == PriorityType.LeftHigher &&
                 part2.GetPriorityType() == PriorityType.Same) return 0;
            else if (part1.GetPriorityType() == PriorityType.RightHigher &&
                part2.GetPriorityType() == PriorityType.Same) return 0;
            else if (part1.GetPriorityType() == PriorityType.Same &&
                part2.GetPriorityType() == PriorityType.LeftHigher) return -1;
            else if (part1.GetPriorityType() == PriorityType.Same &&
                part2.GetPriorityType() == PriorityType.RightHigher) return -1;

            return 0;
        }
    }
}
