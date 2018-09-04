using System;
using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    class FunctionParts : List<FunctionPart>
    {
        private static IReadOnlyList<FunctionPart> allTypes;

        public static IReadOnlyList<FunctionPart> AllTypes
        {
            get
            {
                if (allTypes == null) allTypes = GetAllTypes();

                return allTypes;
            }
        }

        private static IReadOnlyList<FunctionPart> GetAllTypes()
        {
            List<FunctionPart> allTypes = new List<FunctionPart>();

            allTypes.Add(new PartOpenBracket());
            allTypes.Add(new PartCloseBracket());
            allTypes.Add(new PartAcos());
            allTypes.Add(new PartAsin());
            allTypes.Add(new PartAtan());
            allTypes.Add(new PartAbs());
            allTypes.Add(new PartLg());
            allTypes.Add(new PartLn());
            allTypes.Add(new PartPow());
            allTypes.Add(new PartRoot());
            allTypes.Add(new PartLog());
            allTypes.Add(new PartAdd());
            allTypes.Add(new PartSub());
            allTypes.Add(new PartDiv());
            allTypes.Add(new PartMult());
            allTypes.Add(new PartVariable());
            allTypes.Add(new PartConstants(Math.PI, "π", "pi"));
            allTypes.Add(new PartConstants(Math.PI / 180.0, "(π ÷ 180)", "(pi / 180)", "°"));
            allTypes.Add(new PartConstants(Math.E, "e"));

            return allTypes.AsReadOnly();
        }

        public string Equation
        {
            get
            {
                string equation = "";

                foreach (FunctionPart part in this) if (part != null) equation += part.ToEquationString();

                return equation;
            }
        }

        public FunctionParts(int count) : base()
        {
            for (int i = 0; i < count; i++) Add(null);
        }

        public FunctionParts(IEnumerable<FunctionPart> collection) : base(collection) { }

        public FunctionPart Next(int index, PartActionKind kind)
        {
            for (int i = index + 1; i < Count; i++)
            {
                if (this[i].GetActionKind() == kind) return this[i];
            }

            return null;
        }

        public FunctionPart Previous(int index, PartActionKind kind)
        {
            for (int i = index - 1; i >= 0; i--)
            {
                if (this[i].GetActionKind() == kind) return this[i];
            }

            return null;
        }

        public List<PartCalc> GetAsOrderedPartCalcs()
        {
            List<PartCalc> orderedPartCalcs = new List<PartCalc>();
            var areCalcParts = this.Where(x => x.GetActionKind() == PartActionKind.CalcStep);

            var areOrderedPartCalc = areCalcParts.OrderBy(x => (x as PartCalc).Level.Value);
            areOrderedPartCalc = areOrderedPartCalc.ThenBy(x => (x as PartCalc).GetKindPriority());
            areOrderedPartCalc = areOrderedPartCalc.ThenBy(x => (x as PartCalc).Level.Id);

            areOrderedPartCalc.ToList().ForEach((FunctionPart part) => { orderedPartCalcs.Add(part as PartCalc); });

            return orderedPartCalcs;
        }
    }
}
