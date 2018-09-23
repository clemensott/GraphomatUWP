using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class CustomDictionary : ICustomList
    {
        private Graph graph;
        private Dictionary<float, Vector2> dict;

        public CustomDictionary(Graph graph)
        {
            this.graph = graph;
            dict = new Dictionary<float, Vector2>();
        }

        public IEnumerable<Vector2> GetValues(float beginX, float rangeX, float endX)
        {
            float minX = beginX - rangeX / 2f;

            ID id = new ID(beginX, rangeX);
            int endDigits = new ID(endX, rangeX).GetNext().Digits;

            while (id.Digits <= endDigits)
            {
                if (id.Value >= minX)
                {
                    Vector2 value;

                    if (!dict.TryGetValue(id.Value, out value))
                    {
                        value = Calculate(id.Value);
                        dict.Add(id.Value, value);
                    }

                    yield return value;

                    minX += rangeX;
                }

                id.Digits++;
            }
        }

        private Vector2 Calculate(float x)
        {
            return new Vector2(x, (float)graph[x]);
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return dict.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
