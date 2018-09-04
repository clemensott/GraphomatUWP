using System;

namespace MathFunction
{
    abstract class PartPowRoot : PartCalcTwoValue
    {
        public override int GetKindPriority()
        {
            return 4;
        }
    }
}
