using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class FloorListEnumerator : IEnumerator<Vector2>
    {
        private FloorListNode begin;

        public bool Ended { get; private set; }

        public FloorListNode CurrentNode { get; private set; }

        public Vector2 Current { get; private set; }

        object IEnumerator.Current => Current;

        public FloorListEnumerator(FloorListNode begin)
        {
            this.begin = begin;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (Ended) return false;

            CurrentNode = CurrentNode?.Next ?? begin;

            Ended = CurrentNode == null;
            return !Ended;
        }

        public void Reset()
        {
            CurrentNode = null;
            Ended = false;
        }
    }
}
