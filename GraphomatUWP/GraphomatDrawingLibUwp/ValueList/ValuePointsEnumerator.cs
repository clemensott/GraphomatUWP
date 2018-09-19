using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp.Values
{
    class ValuePointsEnumerator : IEnumerator<Vector2>
    {
        public ValuePointNode firstNode;

        public ValuePointNode PreviousNode { get; private set; }

        public ValuePointNode CurrentNode { get; private set; }

        public Vector2 Current { get { return CurrentNode.Value; } }

        object IEnumerator.Current => Current;

        public ValuePointsEnumerator(ValuePointNode first)
        {
            this.firstNode = first;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            PreviousNode = CurrentNode;
            CurrentNode = CurrentNode.Next;

            return CurrentNode == null;
        }

        public void Reset()
        {
            PreviousNode = null;
            CurrentNode = firstNode;
        }

        public void RefreshCurrentNode()
        {
            CurrentNode = PreviousNode.Next;
        }
    }
}
