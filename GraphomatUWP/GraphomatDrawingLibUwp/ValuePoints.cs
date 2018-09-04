using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatDrawingLibUwp
{
    class ValuePoints : IEnumerable<Vector2>
    {
        private float minX, deltaX;
        private Vector2[] points;
        private Graph graph;

        public Vector2 this[int index] { get { return points[index]; } }

        public int Count { get { return points.Length; } }

        public ValuePoints(Graph graph) : base()
        {
            this.graph = graph;
        }

        public void Calculate(ViewArgs args)
        {
            int pointsCount = Convert.ToInt32(args.ViewPixelSize.ActualPixelSize.X) + 1;

            points = new Vector2[pointsCount];

            minX = args.ViewDimensions.TopLeftValuePoint.X;
            deltaX = args.ViewDimensions.ViewValueSize.X / pointsCount;

            Parallel.For(0, pointsCount, new Action<int, ParallelLoopState>(Calculate));
        }

        public void Recalculate(ViewArgs args)
        {
            int pointsCount = Convert.ToInt32(args.ViewPixelSize.RawPixelWidth *
                DrawControl.PixelBufferFactor * DrawControl.PixelBufferFactor);

            points = new Vector2[pointsCount];

            minX = args.ViewDimensions.MiddleOfViewValuePoint.X -
                args.ViewDimensions.ViewValueSize.X * DrawControl.PixelBufferFactor / 2;
            deltaX = args.ViewDimensions.ViewValueSize.X * DrawControl.PixelBufferFactor / pointsCount;

            Parallel.For(0, pointsCount, new Action<int, ParallelLoopState>(Calculate));
        }

        private void Calculate(int index, ParallelLoopState pls)
        {
            float x = minX + deltaX * index;
            float y = (float)graph.GetResult(x);

            points[index] = new Vector2(x, y);
        }

        public IEnumerator<Vector2> GetEnumerator()
        {
            return points.ToList().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
