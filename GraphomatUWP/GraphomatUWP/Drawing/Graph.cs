using MathFunction;
using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;

namespace GraphomatUWP
{
    public class Graph
    {
        private const float thickness = 1;

        private Color color;
        private Function function;

        private List<Vector2> valuePoints;
        private List<Vector2> drawPoints;
        private ViewDimensions lastUpdatedValuesViewDimensions;

        public string OriginalEquation
        {
            get { return function.Equation; }
            set { Function.TryParse(value, out function); }
        }

        public string ImprovedEquation { get { return function.ImprovedEquation; } }

        public Color Color
        {
            get { return color; }
            set
            {

            }
        }

        public Graph(string equation, Color color)
        {
            OriginalEquation = equation;
            this.color = color;

            valuePoints = new List<Vector2>();
            drawPoints = new List<Vector2>();

            lastUpdatedValuesViewDimensions = MoveScollManager.Current.CurrentViewDimensions;
            CalculateValues(lastUpdatedValuesViewDimensions);
            RecalculateDrawPoints(lastUpdatedValuesViewDimensions);

            MoveScollManager.Current.MoveScrollView += MoveScrollManager_MoveScrollView;
        }

        private void MoveScrollManager_MoveScrollView(object sender, MoveScrollEventArgs e)
        {
            float outOfLastViewAreaX = (e.OverRender - 1) / 2 *
                lastUpdatedValuesViewDimensions.ViewValueSize.X;

            if (lastUpdatedValuesViewDimensions.ViewValueSize.X * e.OverRender <
                e.ViewDimensions.ViewValueSize.X || e.ViewDimensions.ViewValueSize.X * e.OverRender <
                lastUpdatedValuesViewDimensions.ViewValueSize.X || e.ViewDimensions.TopLeftValuePoint.X <
                lastUpdatedValuesViewDimensions.TopLeftValuePoint.X - outOfLastViewAreaX / 2 ||
                lastUpdatedValuesViewDimensions.BottomRightValuePoint.X + outOfLastViewAreaX / 2 <
                 e.ViewDimensions.BottomRightValuePoint.X)
            {
                CalculateValues(e.ViewDimensions);
                lastUpdatedValuesViewDimensions = e.ViewDimensions;
            }

            RecalculateDrawPoints(e.ViewDimensions);
        }

        private void CalculateValues(ViewDimensions viewDimensions)
        {
            System.Diagnostics.Debug.WriteLine("Values" + DateTime.Now.Millisecond);
            float minX, deltaX;
            int valuePointsCount = Convert.ToInt32(MoveScollManager.Current.RawPixelSize.X *
                MoveScollManager.OverRender * MoveScollManager.OverRender);
            List<Task<double>> tasks = new List<Task<double>>();

            ChangeValuePointsSize(valuePointsCount);

            minX = viewDimensions.MiddleOfViewValuePoint.X -
                viewDimensions.ViewValueSize.X * MoveScollManager.OverRender / 2;
            deltaX = viewDimensions.ViewValueSize.X * MoveScollManager.OverRender / valuePoints.Count;


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

                point.Y = Convert.ToSingle(tasks[i].Result);
                valuePoints[i] = point;
            }
        }

        private async Task<double> CalculateAsync(double x)
        {
            return function[x];
        }

        private void RecalculateDrawPoints(ViewDimensions viewDimensions)
        {
            float valuePerActualPixelY, originFromTopY, xPixelPerPoint;
            int valuePointsIndex, valuePointsLastIndex, drawPointsCount = 0;

            valuePerActualPixelY = viewDimensions.ViewValueSize.Y / viewDimensions.ActualPixelSize.Y;
            originFromTopY = viewDimensions.TopLeftValuePoint.Y / valuePerActualPixelY * -1;

            valuePointsIndex = valuePoints.FindLastIndex(x => x.X < viewDimensions.TopLeftValuePoint.X);
            valuePointsLastIndex = valuePoints.FindIndex(x => x.X > viewDimensions.BottomRightValuePoint.X);

            xPixelPerPoint = viewDimensions.ActualPixelSize.X / (valuePointsLastIndex - valuePointsIndex);

            while (valuePointsIndex <= valuePointsLastIndex)
            {
                float currentDrawPointY = originFromTopY - valuePoints[valuePointsIndex].Y / valuePerActualPixelY;

                SetDrawPointsValue(drawPointsCount, (drawPointsCount - 1) * xPixelPerPoint, currentDrawPointY);

                drawPointsCount++;
                valuePointsIndex++;
            }

            ChangeDrawPointsSize(drawPointsCount);
        }

        private void ChangeValuePointsSize(int size)
        {
            while (valuePoints.Count > size) valuePoints.RemoveAt(0);
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
            while (drawPoints.Count > size) drawPoints.RemoveAt(0);
            while (drawPoints.Count < size) drawPoints.Add(new Vector2());
        }

        public void Draw(CanvasDrawingSession drawingSession)
        {
            Draw(drawingSession, MoveScollManager.Current.ActualPixelSize);
        }

        public void Draw(CanvasDrawingSession drawingSession, Vector2 actualPixelSize)
        {
            if (drawPoints.Count == 0) return;

            Vector2 previousPoint = drawPoints[0];

            foreach (Vector2 currentPoint in drawPoints)
            {
                if (IsInView(previousPoint, currentPoint, actualPixelSize))
                {
                    drawingSession.DrawLine(previousPoint, currentPoint, color, thickness);
                }

                previousPoint = currentPoint;
            }
        }

        private bool IsInView(Vector2 point1, Vector2 point2, Vector2 pixelSize)
        {
            return !((point1.Y + thickness / 2 < 0 && point2.Y + thickness / 2 < 0) ||
                (point1.Y - thickness / 2 > pixelSize.Y && point2.Y - thickness / 2 > pixelSize.Y));
        }
    }
}
