using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace GraphomatDrawingLibUwp.CustomList
{
    class CalcList : ICustomList
    {
        private Graph graph;

        public CalcList(Graph graph)
        {
            this.graph = graph;
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Vector2> GetValues(float beginX, float rangeX, float endX)
        {
            //return GetValuesSync(beginX, rangeX, endX);

            return GetValuesParallel(beginX, rangeX, endX);
        }

        private IEnumerable<Vector2> GetValuesSync(float beginX, float rangeX, float endX)
        {
            float x = beginX;

            while (x <= endX)
            {
                yield return Calculate(x);

                x += rangeX;
            }
        }

        private IEnumerable<Vector2> GetValuesParallel(float beginX, float rangeX, float endX)
        {
            int count = (int)Math.Floor((endX - beginX) / rangeX) + 1;

            Vector2[] points = new Vector2[count];

            Parallel.For(0, points.Length, (i) => points[i] = Calculate(beginX + rangeX * i));

            return points;
        }

        private Vector2 Calculate(float x)
        {
            return new Vector2(x, (float)graph[x]);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
