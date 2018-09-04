using System.Collections.Generic;
using System.Linq;

namespace MathFunction
{
    class FunctionParts : List<FunctionPart>
    {
        public string Equation
        {
            get
            {
                string equation = "";

                foreach (FunctionPart part in this) equation += part.ToEquationString();

                return equation;
            }
        }

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
