using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;

namespace GraphomatDrawingLibUwp
{
    internal class GraphDrawer
    {
        private const int movingSkipPoints = 10;
        private const float thickness = 2.5F, nearDistance = 20;

        private bool isMoving, valuesAreOutdated;
        private ValuePoints valuePoints;
        private PixelPointsManager pixelManager;
        private ViewDimensions lastUpdatedValuesViewDimensions;
        private Graph graph;
        private ViewArgs preViewArgs;

        public bool IsMoving
        {
            get { return isMoving; }
            set
            {
                isMoving = value;

                if (!isMoving && valuesAreOutdated) MoveScrollView(preViewArgs);
            }
        }

        public Graph Graph { get { return graph; } }

        internal GraphDrawer(Graph graph, ViewArgs args)
        {
            this.graph = graph;
            preViewArgs = args;

            valuesAreOutdated = true;
            valuePoints = new ValuePoints(graph);
            pixelManager = new PixelPointsManager(valuePoints);

            valuePoints.Recalculate(args);
            pixelManager.Recalculate(args);

            lastUpdatedValuesViewDimensions = args.ViewDimensions;
        }

        internal void Move(Vector2 deltaValue)
        {
            Vector2 deltaPixel = deltaValue / preViewArgs.ViewDimensions.ViewValueSize *
                preViewArgs.ViewPixelSize.ActualPixelSize;

            pixelManager.Points.Offset -= deltaPixel;
        }

        internal void MoveScrollView(ViewArgs args)
        {
            valuesAreOutdated = AreValuesOutDated(args);

            if (valuesAreOutdated)
            {
                //if (IsMoving)
                //{
                //    System.Diagnostics.Debug.WriteLine("Calc" + DateTime.Now.Millisecond);
                //    valuePoints.Calculate(args);
                //    //pixelPoints.Calculate(args);
                //}
                //else
                {
                    valuePoints.Recalculate(args);

                    lastUpdatedValuesViewDimensions = args.ViewDimensions;
                    valuesAreOutdated = false;
                }

                pixelManager.Points.Recalculate(args);
            }
            else if (args.ViewDimensions.ViewValueSize == preViewArgs.ViewDimensions.ViewValueSize)
            {
                Vector2 deltaValue = args.ViewDimensions.MiddleOfViewValuePoint -
                    preViewArgs.ViewDimensions.MiddleOfViewValuePoint;

                Move(deltaValue);
            }
            else pixelManager.Points.Recalculate(args);

            preViewArgs = args;
        }

        private bool AreValuesOutDated(ViewArgs e)
        {
            float outOfLastViewAreaX = (e.BufferFactor - 1) / 2 *
                lastUpdatedValuesViewDimensions.ViewValueSize.X;

            if (lastUpdatedValuesViewDimensions.ViewValueSize.X * e.BufferFactor <
                e.ViewDimensions.ViewValueSize.X) return true;

            if (e.ViewDimensions.ViewValueSize.X * e.BufferFactor <
               lastUpdatedValuesViewDimensions.ViewValueSize.X) return true;

            if (e.ViewDimensions.TopLeftValuePoint.X <
                lastUpdatedValuesViewDimensions.TopLeftValuePoint.X - outOfLastViewAreaX) return true;

            return lastUpdatedValuesViewDimensions.BottomRightValuePoint.X + outOfLastViewAreaX <
                 e.ViewDimensions.BottomRightValuePoint.X;
        }

        public void Draw2(ICanvasResourceCreator iCreater,
            CanvasDrawingSession drawingSession, Vector2 actualPixelSize, bool isSelected)
        {
            pixelManager.BeginUsing();

            bool figureEnded = true;
            int i = -1, addToI, pixelPointsCount = pixelManager.Points.Count;
            float curThickness = thickness * (isSelected ? 2 : 1);
            Vector2 curPoint;
            CanvasPathBuilder cpb = new CanvasPathBuilder(iCreater);

            addToI = IsMoving ? 1 + movingSkipPoints : 1;

            do
            {
                i++;

                if (i == pixelPointsCount)
                {
                    pixelManager.EndUsing();
                    return;
                }

                curPoint = pixelManager.Points[i];

            } while (curPoint.X < 0 || float.IsNaN(curPoint.Y));

            while (curPoint.X < actualPixelSize.X && i < pixelPointsCount)
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

            drawingSession.DrawGeometry(CanvasGeometry.CreatePath(cpb), Graph.Color, curThickness);
        }
        /*
        public void Draw(ICanvasResourceCreator iCreater,
            CanvasDrawingSession drawingSession, Vector2 actualPixelSize, bool isSelected)
        {   //  ChecksIfPointsAreInView
            List<Vector2> PixelPointsCur = PixelPoints.BeginDrawing();
            bool figureEnded = true;
            int endIndex, i, addToI;
            float curThickness = thickness * (isSelected ? 2 : 1);
            CanvasPathBuilder cpb = new CanvasPathBuilder(iCreater);

            i = PixelPointsCur.FindIndex(x => x.X >= 0 && !float.IsNaN(x.Y));
            endIndex = PixelPointsCur.FindIndex(x => x.X > actualPixelSize.X);
            addToI = IsMoving ? 1 + movingSkipPoints : 1;

            if (i == -1) return;
            if (endIndex == -1) endIndex = PixelPointsCur.Count;

            while (i < endIndex)
            {
                Vector2 curPoint = GetDrawPoint(PixelPointsCur[i]);
                bool isntCurPointYNaN = !float.IsNaN(curPoint.Y);

                if (figureEnded)
                {
                    if (isntCurPointYNaN && IsInView(curPoint, actualPixelSize))
                    {
                        cpb.BeginFigure(curPoint);
                        figureEnded = false;
                    }
                }
                else if (isntCurPointYNaN)
                {
                    if (IsInView(curPoint, actualPixelSize)) cpb.AddLine(curPoint);
                    else
                    {
                        cpb.EndFigure(CanvasFigureLoop.Open);
                        figureEnded = true;
                    }
                }
                else
                {
                    cpb.EndFigure(CanvasFigureLoop.Open);
                    figureEnded = true;
                }

                i += addToI;
            }

            if (!figureEnded) cpb.EndFigure(CanvasFigureLoop.Open);

            drawingSession.DrawGeometry(CanvasGeometry.CreatePath(cpb), Graph.Color, curThickness);
        }

        private bool IsInView(Vector2 point, Vector2 pixelSize)
        {
            return !(point.Y + thickness / 2 < 0 || point.Y - thickness / 2 > pixelSize.Y);
        }               //              */

        public float IsNearCurve(Vector2 vector)
        {
            pixelManager.BeginUsing();

            int index = -1;
            float minX, maxX, minDistance;
            Vector2 curPoint;

            minX = vector.X - nearDistance;
            maxX = vector.X + nearDistance;
            minDistance = float.MaxValue;

            do
            {
                index++;

                if (index == pixelManager.Points.Count)
                {
                    pixelManager.EndUsing();
                    return minDistance;
                }
                curPoint = pixelManager.Points[index];

            } while (curPoint.X < minX);

            bool? startedAbove = null, switched = false;

            if (!float.IsNaN(curPoint.Y)) startedAbove = curPoint.Y < vector.Y;

            while (index < pixelManager.Points.Count && curPoint.X < vector.X + nearDistance)
            {
                Vector2 distanceVector = curPoint - vector;
                float distance = distanceVector.Length();

                if (distance < minDistance) minDistance = distance;
                if (minDistance > nearDistance && startedAbove == (0 < distanceVector.Y) &&
                    !float.IsNaN(distance)) minDistance = nearDistance;

                if (minDistance < distanceVector.X) break;

                index++;
            }

            if (minDistance > nearDistance) minDistance = switched == true ? nearDistance : float.MaxValue;

            pixelManager.EndUsing();
            return minDistance;
        }

        public void GetMinAndMaxValue(out float min, out float max)
        {
            float minX, maxX;

            minX = preViewArgs.ViewDimensions.TopLeftValuePoint.X;
            maxX = preViewArgs.ViewDimensions.BottomRightValuePoint.X;

            min = float.MaxValue;
            max = float.MinValue;

            foreach (Vector2 point in valuePoints)
            {
                if (point.X >= minX && point.X <= maxX)
                {
                    if (max < point.Y) max = point.Y;
                    if (min > point.Y) min = point.Y;
                }
            }

            if (min != max) return;

            min = -1;
            max = 1;
        }

        public override string ToString()
        {
            return Graph.Name;
        }
    }
}
