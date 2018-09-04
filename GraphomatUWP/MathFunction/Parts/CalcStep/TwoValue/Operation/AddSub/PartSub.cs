using System.Collections.Generic;

namespace MathFunction
{
    class PartSub : PartAddSub
    {
        public override string[] GetLowerLooks()
        {
            return new string[] { "-" };
        }

        public override void ChangeToPartSignIfSub(ref FunctionParts parts)
        {
            parts[parts.IndexOf(this)] = new PartSign();
        }

        protected override double Calc()
        {
            return Value1.Value - Value2.Value;
        }

        public override FunctionPart Clone()
        {
            return new PartSub();
        }
    }
}
