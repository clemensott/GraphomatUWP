using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;

namespace GraphomatDrawingLibUwp.ValueList
{
    class ValuePoints : IEnumerable<Vector2>
    {
        private ValuePointNode first;
        private Graph graph;

        private float minX, deltaX;
        private Vector2[] points;

        public ValuePoints(Graph graph) : base()
        {
            this.graph = graph;
        }

        public void Recalculate(ViewArgs args)
        {
            int pointsCount = Convert.ToInt32(args.PixelSize.RawPixelWidth *
                DrawControl.PixelBufferFactor * DrawControl.PixelBufferFactor);

            points = new Vector2[pointsCount];

            minX = args.ValueDimensions.Middle.X -
                args.ValueDimensions.Width * DrawControl.PixelBufferFactor / 2;
            deltaX = args.ValueDimensions.Width * DrawControl.PixelBufferFactor / pointsCount;

            Parallel.For(0, pointsCount, new Action<int, ParallelLoopState>(CalculateIntoPoints));

            if (points.Length > 0)
            {
                ValuePointNode next = new ValuePointNode(null, points[points.Length - 1]);

                for (int i = points.Length - 2; i >= 0; i--)
                {
                    next = new ValuePointNode(next, points[i]);
                }

                first = next;
            }
            else first = null;
        }

        private void CalculateIntoPoints(int index, ParallelLoopState pls)
        {
            points[index] = Calculate(minX + deltaX * index);
        }

        private Vector2 Calculate(float x)
        {
            return new Vector2(x, (float)graph[x]);
        }

        public IEnumerable<Vector2> GetValues(float beginX, float rangeX, float endX)
        {
            float lowerX = beginX - rangeX / 2f;
            float upperX = beginX;

            if (first == null || first.Value.X >= upperX) InsertAtBeginning(Calculate((lowerX + upperX) / 2f));

            ValuePointsEnumerator enumerator = new ValuePointsEnumerator(first);

            while (true)
            {
                while (enumerator.MoveNext() && enumerator.Current.X < lowerX) ;

                if (enumerator.CurrentNode == null) break;

                if (enumerator.Current.X >= upperX)
                {
                    InsertAfter(enumerator.PreviousNode, Calculate((lowerX + upperX) / 2f));
                    enumerator.RefreshCurrentNode();
                }

                yield return enumerator.Current;

                if (lowerX >= endX) yield break;

                lowerX += rangeX;
                upperX += rangeX;
            }

            ValuePointNode last = enumerator.PreviousNode;

            while (true)
            {
                last = InsertAfter(last, Calculate((lowerX + upperX) / 2f));

                yield return last.Value;

                if (lowerX >= endX) yield break;

                lowerX += rangeX;
                upperX += rangeX;
            }
        }

        private void InsertAtBeginning(Vector2 point)
        {
            first = new ValuePointNode(first, point);
        }

        private ValuePointNode InsertAfter(ValuePointNode node, Vector2 point)
        {
            return node.Next = new ValuePointNode(node.Next, point);
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return new ValuePointsEnumerator(first);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ValuePointsEnumerator(first);
        }
    }
}
