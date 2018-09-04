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

        public PartValueCalc(PartValue partValue, List<PositionPartValue> positions) : base(partValue.Value)
        {
            Value = startValue = partValue.Value;
            IsVariable = partValue.IsVariable;

            Positions = positions;
        }

        public PartValueCalc(PartValueCalc toClone) : base(toClone.Value)
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
