using System.Collections.Generic;

namespace MathFunction
{
    class CalculatorThread
    {
        private object lockObj;

        public List<PartValueCalc> Values { get; private set; }

        public List<PartCalc> Steps { get; private set; }

        public object LockObject { get { return lockObj; } }

        public CalculatorThread(List<PartCalc> orderedPartCalcs, List<PartValueCalc> toCloneValues)
        {
            lockObj = new object();

            CloneSteps(orderedPartCalcs);
            CloneAndSetValues(toCloneValues);
        }

        private void CloneSteps(List<PartCalc> orderedPartCalcs)
        {
            Steps = new List<PartCalc>();

            foreach (PartCalc step in orderedPartCalcs)
            {
                Steps.Add(step.Clone());
            }
        }

        private void CloneAndSetValues(List<PartValueCalc> toCloneValues)
        {
            Values = new List<PartValueCalc>();

            foreach (PartValueCalc toCloneValue in toCloneValues)
            {
                PartValueCalc value = new PartValueCalc(toCloneValue);
                Values.Add(value);

                foreach (PositionPartValue position in value.Positions)
                {
                    if (position.IsValue1) Steps[position.PartCalcIndex].Value1 = value;
                    else Steps[position.PartCalcIndex].Value2 = value;
                }
            }
        }

        public double GetResult(double x)
        {
            ResetValues(x);

            foreach (PartCalc step in Steps) step.Do();

            return Values[0].Value;
        }

        private void ResetValues(double x)
        {
            foreach(PartValueCalc value in Values)
            {
                value.Reset(x);
            }
        }
    }
}
