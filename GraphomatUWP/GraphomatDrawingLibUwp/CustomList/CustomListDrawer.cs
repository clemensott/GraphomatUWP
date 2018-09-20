using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    internal abstract class CustomListDrawer : GraphDrawer
    {
        protected ValuePointLinkedList valuePointList;

        public CustomListDrawer(Graph graph, ViewArgs args) : base(graph, args)
        {
            valuePointList = new ValuePointLinkedList(graph);
        }

        protected abstract ICustomList CreateValuePointList();

        public override CanvasGeometry Draw(ICanvasResourceCreator iCreater, bool isMoving)
        {
            if (ViewArgs.PixelSize.RawPixelWidth == 0) return null;

            bool reachedEnd = false;
            CanvasPathBuilder cpb = new CanvasPathBuilder(iCreater);

            float beginX = ViewArgs.ValueDimensions.Left;
            float rangeX = ViewArgs.ValueDimensions.Width / ViewArgs.PixelSize.RawPixelWidth;
            float endX = ViewArgs.ValueDimensions.Right;

            if (isMoving) rangeX *= (1 + movingSkipPoints);

            IEnumerator<Vector2> enumerator = valuePointList.GetValues(beginX, rangeX, endX).GetEnumerator();

            while (!reachedEnd)
            {
                while (!reachedEnd)
                {
                    if (!enumerator.MoveNext()) reachedEnd = true;
                    else if (!float.IsNaN(enumerator.Current.Y)) break;
                }

                cpb.BeginFigure(ToViewPoint(enumerator.Current));

                while (!reachedEnd)
                {
                    if (!enumerator.MoveNext()) reachedEnd = true;
                    else if (float.IsNaN(enumerator.Current.Y)) break;

                    cpb.AddLine(ToViewPoint(enumerator.Current));
                }

                cpb.EndFigure(CanvasFigureLoop.Open);
            }

            return CanvasGeometry.CreatePath(cpb);
        }

        protected override IEnumerable<Vector2> GetPoints()
        {
            if (ViewArgs.PixelSize.RawPixelWidth == 0) return valuePointList;

            float beginX = ViewArgs.ValueDimensions.Left;
            float rangeX = ViewArgs.ValueDimensions.Width / ViewArgs.PixelSize.RawPixelWidth;
            float endX = ViewArgs.ValueDimensions.Right;

            return valuePointList.GetValues(beginX, rangeX, endX);
        }
    }
}
