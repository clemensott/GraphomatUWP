using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class FloorList : ICustomList
    {
        private Graph graph;
        private FloorListNode root;

        public FloorList(Graph graph)
        {
            this.graph = graph;
        }

        public IEnumerable<Vector2> GetValues(float beginX, float rangeX, float endX)
        {
            if (root == null)
            {
                
            }

            FloorListNode node = root;



            IEnumerator<Vector2> enumerator = new FloorListEnumerator(node);

            while (enumerator.MoveNext()) yield return enumerator.Current;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return new FloorListEnumerator(root);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
