using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    abstract class PartOperation : PartCalcTwoValue
    {
        public override bool IsChangeOrderAble()
        {
            return true;
        }

        public override string ToEquationString()
        {
            return " " + GetLowerLooks()[0] + " ";
        }
    }
}
