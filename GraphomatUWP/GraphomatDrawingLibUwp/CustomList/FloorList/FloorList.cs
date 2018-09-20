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

        public FloorList(Graph graph)
        {
            this.graph = graph;
        }

        public IEnumerable<Vector2> GetValues(float beginX, float rangeX, float endX)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
