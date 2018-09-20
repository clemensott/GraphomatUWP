using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

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
    }
}
