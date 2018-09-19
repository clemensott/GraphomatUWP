using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp.ValueList
{
    class ValuePointsEnumerator : IEnumerator<Vector2>
    {
        public ValuePointNode firstNode;

        public ValuePointNode PreviousNode { get; private set; }

        public ValuePointNode CurrentNode { get; private set; }

        public Vector2 Current { get; private set; }

        object IEnumerator.Current => Current;

        public ValuePointsEnumerator(ValuePointNode first)
        {
            firstNode = first;

            Reset();
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            PreviousNode = CurrentNode;
            CurrentNode = CurrentNode.Next;
            Current = CurrentNode?.Value ?? default(Vector2);

            return CurrentNode != null;
        }

        public void Reset()
        {
            PreviousNode = null;
            CurrentNode = new ValuePointNode(firstNode, default(Vector2));
        }

        public void RefreshCurrentNode()
        {
            CurrentNode = PreviousNode.Next;
            Current = CurrentNode.Value;
        }
    }
}
