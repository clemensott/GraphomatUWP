using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class CustomDictionary : ICustomList
    {
        private Graph graph;
        private Dictionary<ID, Vector2> dict;

        public CustomDictionary(Graph graph)
        {
            this.graph = graph;
            dict = new Dictionary<ID, Vector2>();
        }

        public IEnumerable<Vector2> GetValues(float beginX, float rangeX, float endX)
        {
            ID id = new ID(beginX, rangeX);
            int endDigits = new ID(endX, rangeX).GetNext().Digits;
            int rangeDigits = (int)((endDigits - id.Digits) / ((endX - beginX) / rangeX));
            int minDigits = id.Digits;

            while (id.Digits <= endDigits)
            {
                if (id.Digits >= minDigits)
                {
                    Vector2 value;

                    if (!dict.TryGetValue(id, out value))
                    {
                        value = Calculate(id.GetValue());
                        dict.Add(id, value);
                    }

                    yield return value;

                    minDigits += rangeDigits;
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
