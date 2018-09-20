using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GraphomatDrawingLibUwp.ArrayManagement
{
    class ArrayDrawer : GraphDrawer
    {
        private bool valuesAreOutdated;
        private ValuePoints valuePoints;
        private PixelPointsManager pixelManager;
        private ViewValueDimensions lastUpdatedValuesValueDimensions;
        private Graph graph;
        private ViewArgs preViewArgs;

        internal ArrayDrawer(Graph graph, ViewArgs args) : base(graph, args)
        {
            this.graph = graph;
            preViewArgs = args;

            valuesAreOutdated = true;
            valuePoints = new ValuePoints(graph);
            pixelManager = new PixelPointsManager(valuePoints);

            valuePoints.Recalculate(args);
            pixelManager.Recalculate(args);

            lastUpdatedValuesValueDimensions = args.ValueDimensions;
        }

        protected override void Move(Vector2 deltaValue)
        {
            Vector2 deltaPixel = deltaValue / preViewArgs.ValueDimensions.Size *
                preViewArgs.PixelSize.ActualPixelSize;

            pixelManager.Points.Offset -= deltaPixel;
        }

        protected override void MoveScrollView()
        {
            if (valuePoints == null) return;

            valuesAreOutdated = AreValuesOutDated(ViewArgs);

            if (valuesAreOutdated)
            {
                valuePoints.Recalculate(ViewArgs);

                lastUpdatedValuesValueDimensions = ViewArgs.ValueDimensions;
                valuesAreOutdated = false;

                pixelManager.Points.Recalculate(ViewArgs);
            }
            else if (ViewArgs.ValueDimensions.Size == preViewArgs.ValueDimensions.Size)
            {
                Vector2 deltaValue = ViewArgs.ValueDimensions.Middle -
                    preViewArgs.ValueDimensions.Middle;

                Move(deltaValue);
            }
            else pixelManager.Points.Recalculate(ViewArgs);

            preViewArgs = ViewArgs;
        }

        private bool AreValuesOutDated(ViewArgs e)
        {
            float outOfLastViewAreaX = (e.BufferFactor - 1) / 2 *
                lastUpdatedValuesValueDimensions.Width;

            if (lastUpdatedValuesValueDimensions.Width * e.BufferFactor <
                e.ValueDimensions.Width) return true;

            if (e.ValueDimensions.Width * e.BufferFactor <
               lastUpdatedValuesValueDimensions.Width) return true;

            if (e.ValueDimensions.Left <
                lastUpdatedValuesValueDimensions.Left - outOfLastViewAreaX) return true;

            return lastUpdatedValuesValueDimensions.Right + outOfLastViewAreaX <
                 e.ValueDimensions.Right;
        }

        public override CanvasGeometry Draw(ICanvasResourceCreator iCreater, bool isMoving)
        {
            pixelManager.BeginUsing();

            bool figureEnded = true;
            int i = -1, addToI, pixelPointsCount = pixelManager.Points.Count;
            float pixelWidth = ViewArgs.PixelSize.ActualWidth;
            Vector2 curPoint;
            CanvasPathBuilder cpb = new CanvasPathBuilder(iCreater);

            addToI = isMoving ? 1 + movingSkipPoints : 1;

            do
            {
                i++;

                if (i == pixelPointsCount)
                {
                    pixelManager.EndUsing();
                    return null;
                }

                curPoint = pixelManager.Points[i];

            } while (curPoint.X < 0 || float.IsNaN(curPoint.Y));

            while (curPoint.X < pixelWidth && i < pixelPointsCount)
            {
                curPoint = pixelManager.Points[i];

                if (figureEnded)
                {
                    if (!float.IsNaN(curPoint.Y))
                    {
                        cpb.BeginFigure(curPoint);
                        figureEnded = false;
                    }
                }
                else if (!float.IsNaN(curPoint.Y)) cpb.AddLine(curPoint);
                else
                {
                    cpb.EndFigure(CanvasFigureLoop.Open);
                    figureEnded = true;
                }

                i += addToI;
            }

            pixelManager.EndUsing();

            if (!figureEnded) cpb.EndFigure(CanvasFigureLoop.Open);

            return CanvasGeometry.CreatePath(cpb);
        }

        protected override IEnumerable<Vector2> GetPoints()
        {
            pixelManager.BeginUsing();
            Vector2[] array = pixelManager.Points.ToArray();
            pixelManager.EndUsing();

            return array;
        }
    }
}
