using System;
using System.Collections.Generic;
using System.Threading;

namespace MathFunction
{
    class Calculator
    {
        private const int defaultThreadCount = 6;

        private object isCalculatingCheckLock;

        private Random ran;
        private List<CalculatorThread> threads;

        public int ThreadsCount
        {
            get { return threads.Count; }
            set
            {
                if (value <= 0) throw new IndexOutOfRangeException("It is at least one Thread needed.");

                for (int i = threads.Count; i < value; i++) AddThread();
                for (int i = threads.Count; i > value; i--) DeleteThread();
            }
        }

        public Calculator(FunctionParts parts)
        {
            isCalculatingCheckLock = new object();
            ran = new Random();
            threads = new List<CalculatorThread>();

            List<PartValueCalc> calcValues;
            List<PartCalc> orderedPartCalcs;

            GetPartValueCalcs(parts, out calcValues, out orderedPartCalcs);

            threads.Add(new CalculatorThread(orderedPartCalcs, calcValues));

            ThreadsCount = defaultThreadCount;
        }

        private void GetPartValueCalcs(FunctionParts parts,
            out List<PartValueCalc> calcValues, out List<PartCalc> orderedPartCalcs)
        {
            List<PartValue> justValues = new List<PartValue>();
            calcValues = new List<PartValueCalc>();
            orderedPartCalcs = parts.GetAsOrderedPartCalcs();

            foreach (FunctionPart part in parts)
            {
                if (part.GetActionKind() == PartActionKind.Value) justValues.Add(part as PartValue);
            }

            foreach (PartValue justValue in justValues)
            {
                List<PositionPartValue> positions = new List<PositionPartValue>();

                for (int i = 0; i < orderedPartCalcs.Count; i++)
                {
                    if (orderedPartCalcs[i].Value1 == justValue) positions.Add(new PositionPartValue(i, true));
                    else if (orderedPartCalcs[i].Value2 == justValue)
                    {
                        positions.Add(new PositionPartValue(i, false));
                    }
                }

                calcValues.Add(new PartValueCalc(justValue, positions));
            }
        }

        private void AddThread()
        {
            threads.Add(new CalculatorThread(threads[0].Steps, threads[0].Values));
        }

        private void DeleteThread()
        {
            int index = GetFalseIsCalculatingAndSetTrue();
            object lockObject = threads[index].LockObject;

            Monitor.Enter(lockObject);

            lock (isCalculatingCheckLock)
            {
                threads.RemoveAt(index);
            }

            Monitor.Exit(lockObject);
        }

        public double GetResult(double x)
        {
            int index = GetFalseIsCalculatingAndSetTrue();

            Monitor.Enter(threads[index].LockObject);

            double result = threads[index].GetResult(x);

            Monitor.Exit(threads[index].LockObject);

            return result;
        }

        private int GetFalseIsCalculatingAndSetTrue()
        {
            int index;

            lock (isCalculatingCheckLock)
            {
                for (index = 0; index < ThreadsCount; index++)
                {
                    if (Monitor.IsEntered(threads[index].LockObject)) break;
                }

                if (index == ThreadsCount) index = ran.Next(ThreadsCount);

                Monitor.Enter(threads[index].LockObject);
            }

            return index;
        }
    }
}
