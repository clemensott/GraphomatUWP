using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    class PartValueCalc : PartValue
    {
        private double startValue;

        public List<PositionPartValue> Positions { get; private set; }

        public PartValueCalc(PartValue partValue, List<PositionPartValue> positions)
        {
            Value = startValue = partValue.Value;
            IsVariable = partValue.IsVariable;
            Value = partValue.Value;

            Positions = positions;
        }

        public PartValueCalc(PartValueCalc toClone)
        {
            Value = startValue = toClone.Value;
            IsVariable = toClone.IsVariable;

            Positions = toClone.Positions;
        }

        public void Reset(double variableValue)
        {
            realValue = null;

            Value = IsVariable ? variableValue : startValue;
        }
    }
}
