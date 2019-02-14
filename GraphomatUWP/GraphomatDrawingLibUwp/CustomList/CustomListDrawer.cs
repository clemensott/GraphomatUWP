using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    internal abstract class CustomListDrawer<T> : GraphDrawer where T : ICustomList
    {
        protected T valuePointList;

        public CustomListDrawer(Graph graph, ViewArgs args) : base(graph, args)
        {
            valuePointList = CreateValuePointList();
        }

        protected abstract T CreateValuePointList();

        public override CanvasGeometry Draw(ICanvasResourceCreator iCreater, bool isMoving)
        {
            if (ViewArgs.PixelSize.RawPixelWidth == 0) return null;

            CanvasPathBuilder cpb = new CanvasPathBuilder(iCreater);

            float beginX = ViewArgs.ValueDimensions.Left;
            float rangeX = ViewArgs.ValueDimensions.Width / ViewArgs.PixelSize.RawPixelWidth / 1f;
            float endX = ViewArgs.ValueDimensions.Right;

            if (isMoving) rangeX *= (1 + movingSkipPoints);

            IEnumerable<Vector2> valuePoints = valuePointList.GetValues(beginX, rangeX, endX);
            IEnumerable<IEnumerable<Vector2>> valueSections = GetSections(valuePoints);
            IEnumerable<IEnumerable<Vector2>> viewSections = valueSections.Select(s => s.Select(ToViewPoint));

            foreach (IEnumerable<Vector2> section in viewSections)
            {
                IEnumerator<Vector2> enumerator = section.GetEnumerator();
                enumerator.MoveNext();

                cpb.BeginFigure(enumerator.Current);

                while (enumerator.MoveNext()) cpb.AddLine(enumerator.Current);

                cpb.EndFigure(CanvasFigureLoop.Open);
            }

            return CanvasGeometry.CreatePath(cpb);
        }

        private IEnumerable<IEnumerable<Vector2>> GetSections(IEnumerable<Vector2> points)
        {
            bool ended = false;
            IEnumerator<Vector2> enumerator = points.GetEnumerator();

            while (!ended)
            {
                yield return getSection();
            }

            IEnumerable<Vector2> getSection()
            {
                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        ended = true;
                        yield break;
                    }

                    if (float.IsNaN(enumerator.Current.Y)) continue;

                    Vector2 first = enumerator.Current;

                    if (!enumerator.MoveNext())
                    {
                        ended = true;
                        yield break;
                    }

                    if (float.IsNaN(enumerator.Current.Y)) continue;

                    yield return first;
                    yield return enumerator.Current;

                    break;
                }

                while (true)
                {
                    if (!enumerator.MoveNext())
                    {
                        ended = true;
                        yield break;
                    }

                    if (float.IsNaN(enumerator.Current.Y)) yield break;

                    yield return enumerator.Current;
                }
            }
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
