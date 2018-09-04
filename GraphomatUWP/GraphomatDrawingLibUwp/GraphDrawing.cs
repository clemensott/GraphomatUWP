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
    public class GraphDrawing
    {
        private const float thickness = 2.5F, nearDistance = 20;

        private int valuePointsStartIndex, valuePointsStopIndex;
        private List<Vector2> valuePoints;
        private List<Vector2> drawPoints;
        private ViewDimensions lastUpdatedValuesViewDimensions;
        private Graph graph;
        private ViewArgs preViewArgs;

        public Graph Graph { get { return graph; } }

        internal GraphDrawing(Graph graph, ViewArgs args)
        {
            this.graph = graph;

            preViewArgs = args;

            valuePoints = new List<Vector2>();
            drawPoints = new List<Vector2>();

            CalculateValues(args);
            RecalculateDrawPoints(args);

            lastUpdatedValuesViewDimensions = args.ViewDimensions;
        }

        internal void Move(Vector2 deltaValue)
        {
            Vector2 deltaPixel = deltaValue / preViewArgs.ViewDimensions.ViewValueSize *
                preViewArgs.ViewPixelSize.ActualPixelSize * new Vector2(-1, -1);

            for (int i = 0; i < drawPoints.Count; i++)
            {
                drawPoints[i] = drawPoints[i] + deltaPixel;
            }
        }

        internal void MoveScrollView(ViewArgs args)
        {
            if (AreValuesOutDated(args))
            {
                CalculateValues(args);
                lastUpdatedValuesViewDimensions = args.ViewDimensions;

                RecalculateDrawPoints(args);
            }
            else if (args.ViewDimensions.ViewValueSize == preViewArgs.ViewDimensions.ViewValueSize)
            {
                Vector2 deltaValue = args.ViewDimensions.MiddleOfViewValuePoint -
                    preViewArgs.ViewDimensions.MiddleOfViewValuePoint;

                Move(deltaValue);
            }
            else RecalculateDrawPoints(args);

            preViewArgs = args;
        }

        private bool AreValuesOutDated(ViewArgs e)
        {
            float outOfLastViewAreaX = (e.OverRender - 1) / 2 *
                lastUpdatedValuesViewDimensions.ViewValueSize.X;

            if (lastUpdatedValuesViewDimensions.ViewValueSize.X * e.OverRender <
                e.ViewDimensions.ViewValueSize.X) return true;

            if (e.ViewDimensions.ViewValueSize.X * e.OverRender <
               lastUpdatedValuesViewDimensions.ViewValueSize.X) return true;

            if (e.ViewDimensions.TopLeftValuePoint.X <
                lastUpdatedValuesViewDimensions.TopLeftValuePoint.X - outOfLastViewAreaX) return true;

            return lastUpdatedValuesViewDimensions.BottomRightValuePoint.X + outOfLastViewAreaX <
                 e.ViewDimensions.BottomRightValuePoint.X;
        }

        private void CalculateValues(ViewArgs args)
        {
            float minX, deltaX;
            int valuePointsCount = Convert.ToInt32(args.ViewPixelSize.RawPixelWidth *
                DrawControl.MoreRenderFactor * DrawControl.MoreRenderFactor);
            List<Task<double>> tasks = new List<Task<double>>();

            ChangeValuePointsSize(valuePointsCount);

            minX = args.ViewDimensions.MiddleOfViewValuePoint.X -
                args.ViewDimensions.ViewValueSize.X * DrawControl.MoreRenderFactor / 2;
            deltaX = args.ViewDimensions.ViewValueSize.X * DrawControl.MoreRenderFactor / valuePoints.Count;


            for (int i = 0; i < valuePoints.Count; i++)
            {
                float x = minX + deltaX * i;
                tasks.Add(CalculateAsync(x));

                Vector2 point = valuePoints[i];
                point.X = x;
                valuePoints[i] = point;
            }

            for (int i = 0; i < valuePoints.Count; i++)
            {
                Vector2 point = valuePoints[i];
                tasks[i].Wait();

                point.Y = (float)tasks[i].Result;
                valuePoints[i] = point;
            }
        }

        private async Task<double> CalculateAsync(double x)
        {
            return Graph[x];
        }

        private void RecalculateDrawPoints(ViewArgs args)
        {
            float valuePerActualPixelY, originFromTopY, selectValueMin, selectValueMax,
                valuePerRawPixelX, rawPixelXPerValues, actualPixelXPerValue, leftSideY;
            int drawPointsIndex = 0;

            valuePerActualPixelY = args.ViewDimensions.ViewValueSize.Y / args.ViewPixelSize.ActualPixelSize.Y;
            originFromTopY = args.ViewDimensions.TopLeftValuePoint.Y / valuePerActualPixelY * -1;

            valuePointsStartIndex = valuePoints.FindLastIndex(x => x.X < args.ViewDimensions.TopLeftValuePoint.X);
            valuePointsStopIndex = valuePoints.FindIndex(x => x.X > args.ViewDimensions.BottomRightValuePoint.X) + 1;

            valuePerRawPixelX = args.ViewDimensions.ViewValueSize.X / args.ViewPixelSize.RawPixelWidth;
            rawPixelXPerValues = args.ViewPixelSize.RawPixelWidth / (valuePointsStopIndex - valuePointsStartIndex);

            selectValueMin = (1 - rawPixelXPerValues) / 2F * valuePerRawPixelX;
            selectValueMax = (1 + rawPixelXPerValues) / 2F * valuePerRawPixelX;

            actualPixelXPerValue = args.ViewPixelSize.ActualPixelSize.X / args.ViewDimensions.ViewValueSize.X;
            leftSideY = args.ViewDimensions.TopLeftValuePoint.X;

            for (int i = 0; i < valuePoints.Count; i++)
            {
                float nearAllowedValue = (valuePoints[i].X % valuePerRawPixelX + valuePerRawPixelX) % valuePerRawPixelX;

                if (selectValueMin <= nearAllowedValue && nearAllowedValue < selectValueMax)
                {
                    float currentDrawPointY = originFromTopY - valuePoints[i].Y / valuePerActualPixelY;
                    float currentDrawPointX = (valuePoints[i].X - leftSideY) * actualPixelXPerValue;

                    SetDrawPointsValue(drawPointsIndex, currentDrawPointX, currentDrawPointY);

                    drawPointsIndex++;
                }
            }

            ChangeDrawPointsSize(drawPointsIndex);
        }

        private void ChangeValuePointsSize(int size)
        {
            while (valuePoints.Count > size) valuePoints.RemoveAt(size);
            while (valuePoints.Count < size) valuePoints.Add(new Vector2());
        }

        private void SetDrawPointsValue(int index, float x, float y)
        {
            if (index < drawPoints.Count)
            {
                drawPoints[index] = new Vector2(x, y);

                return;
            }

            while (drawPoints.Count + 1 < index) valuePoints.Add(new Vector2());

            drawPoints.Add(new Vector2(x, y));
        }

        private void ChangeDrawPointsSize(int size)
        {
            while (drawPoints.Count > size) drawPoints.RemoveAt(size);
            while (drawPoints.Count < size) drawPoints.Add(new Vector2());
        }

        public void Draw2(ICanvasResourceCreator iCreater,
            CanvasDrawingSession drawingSession, Vector2 actualPixelSize, bool isSelected)
        {
            bool figureEnded = true;
            int endIndex, i;
            Vector2 prePoint;
            float curThickness = thickness * (isSelected ? 2 : 1);
            CanvasPathBuilder cpb = new CanvasPathBuilder(iCreater);

            i = drawPoints.FindIndex(x => x.X >= 0 && !float.IsNaN(x.Y));
            endIndex = drawPoints.FindIndex(x => x.X > actualPixelSize.X);

            if (i == -1) return;
            if (endIndex == -1) endIndex = drawPoints.Count;

            prePoint = drawPoints[i];

            while (i < endIndex)
            {
                if (figureEnded)
                {
                    if (!float.IsNaN(drawPoints[i].Y))
                    {
                        cpb.BeginFigure(drawPoints[i]);
                        figureEnded = false;
                    }
                }
                else if (!float.IsNaN(drawPoints[i].Y))
                {
                    cpb.AddLine(drawPoints[i]);
                }
                else
                {
                    cpb.EndFigure(CanvasFigureLoop.Open);
                    figureEnded = true;
                }

                prePoint = drawPoints[i];
                i++;
            }

            if (!figureEnded) cpb.EndFigure(CanvasFigureLoop.Open);

            drawingSession.DrawGeometry(CanvasGeometry.CreatePath(cpb), Graph.Color, curThickness);
        }

        public void Draw(ICanvasResourceCreator iCreater,
            CanvasDrawingSession drawingSession, Vector2 actualPixelSize, bool isSelected)
        {
            bool figureEnded = true;
            int endIndex, i;
            float curThickness = thickness * (isSelected ? 2 : 1);
            CanvasPathBuilder cpb = new CanvasPathBuilder(iCreater);

            i = drawPoints.FindIndex(x => x.X >= 0 && !float.IsNaN(x.Y));
            endIndex = drawPoints.FindIndex(x => x.X > actualPixelSize.X);

            if (i == -1) return;
            if (endIndex == -1) endIndex = drawPoints.Count;

            while (i < endIndex)
            {
                if (figureEnded)
                {
                    if (!float.IsNaN(drawPoints[i].Y) && IsInView(drawPoints[i], actualPixelSize))
                    {
                        cpb.BeginFigure(drawPoints[i]);
                        figureEnded = false;
                    }
                }
                else if (!float.IsNaN(drawPoints[i].Y))
                {
                    if (IsInView(drawPoints[i], actualPixelSize)) cpb.AddLine(drawPoints[i]);
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

                i++;
            }

            if (!figureEnded) cpb.EndFigure(CanvasFigureLoop.Open);

            drawingSession.DrawGeometry(CanvasGeometry.CreatePath(cpb), Graph.Color, curThickness);
        }

        private bool IsInView(Vector2 point, Vector2 pixelSize)
        {
            return !(point.Y + thickness / 2 < 0 || point.Y - thickness / 2 > pixelSize.Y);
        }

        public float IsNearCurve(Vector2 vector)
        {
            int index = drawPoints.FindIndex(x => x.X > vector.X - nearDistance);
            float min = float.MaxValue;

            if (index == -1) return min;

            bool? startedAbove = null, switched = false;

            if (!float.IsNaN(drawPoints[index].Y)) startedAbove = drawPoints[index].Y < vector.Y;

            while (index < drawPoints.Count && drawPoints[index].X < vector.X + nearDistance)
            {
                Vector2 distanceVector = drawPoints[index] - vector;
                float distance = distanceVector.Length();

                if (distance < min) min = distance;
                if (min > nearDistance && startedAbove == (0 < distanceVector.Y) &&
                    !float.IsNaN(distance)) min = nearDistance;

                if (min < distanceVector.X) break;

                index++;
            }

            if (min > nearDistance) min = switched == true ? nearDistance : float.MaxValue;

            return min;
        }

        public void GetMinAndMaxValue(out float min, out float max)
        {
            min = float.MaxValue;
            max = float.MinValue;

            for (int i = valuePointsStartIndex; i < valuePointsStopIndex; i++)
            {
                if (max < valuePoints[i].Y) max = valuePoints[i].Y;
                if (min > valuePoints[i].Y) min = valuePoints[i].Y;
            }
        }

        public override string ToString()
        {
            return Graph.Name;
        }
    }
}
