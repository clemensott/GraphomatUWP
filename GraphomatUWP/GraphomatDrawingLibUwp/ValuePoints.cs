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
            int pointsCount = Convert.ToInt32(args.PixelSize.ActualPixelSize.X) + 1;

            points = new Vector2[pointsCount];

            minX = args.ValueDimensions.Left;
            deltaX = args.ValueDimensions.Width / pointsCount;

            Parallel.For(0, pointsCount, new Action<int, ParallelLoopState>(Calculate));
        }

        public void Recalculate(ViewArgs args)
        {
            int pointsCount = Convert.ToInt32(args.PixelSize.RawPixelWidth *
                DrawControl.PixelBufferFactor * DrawControl.PixelBufferFactor);

            points = new Vector2[pointsCount];

            minX = args.ValueDimensions.Middle.X -
                args.ValueDimensions.Width * DrawControl.PixelBufferFactor / 2;
            deltaX = args.ValueDimensions.Width * DrawControl.PixelBufferFactor / pointsCount;

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
            return ((IEnumerable<Vector2>)points).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
