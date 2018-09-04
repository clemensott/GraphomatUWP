﻿using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;

namespace GraphomatDrawingLibUwp
{
    class AxesGraph
    {
        private const float thickness = 2, strokeLenght = 10;

        private readonly float[] possibleDistanceBetweenStrokes = new float[] { 1, 1.5F, 2, 3, 5, 7 };

        private HorizontalLine xAxisLine;
        private List<VerticalLineText> xAxisStrokes;
        private VerticalLine yAxisLine;
        private List<HorizontalLineText> yAxisStrokes;
        private Color color;
        private ViewArgs preViewArgs;

        public AxesGraph(ViewArgs args)
        {
            preViewArgs = args;

            xAxisLine = new HorizontalLine();
            yAxisLine = new VerticalLine();

            xAxisStrokes = new List<VerticalLineText>();
            yAxisStrokes = new List<HorizontalLineText>();

            SetAxesLines(args);
            SetXAxisStrokes(args);
            SetYAxisStrokes(args);

            color = Color.FromArgb(255, 0, 0, 0);
        }

        public void MoveScrollView(ViewArgs args)
        {
            SetAxesLines(args);
            SetXAxisStrokes(args);
            SetYAxisStrokes(args);
        }

        private void SetAxesLines(ViewArgs args)
        {
            float valuePerAcutalPixelX, valuePerAcutalPixelY;

            valuePerAcutalPixelX = args.ViewDimensions.ViewValueSize.X / args.ViewPixelSize.ActualPixelSize.X;
            valuePerAcutalPixelY = args.ViewDimensions.ViewValueSize.Y / args.ViewPixelSize.ActualPixelSize.Y;

            xAxisLine.Y = args.ViewDimensions.TopLeftValuePoint.Y / valuePerAcutalPixelY * -1;
            yAxisLine.X = args.ViewDimensions.TopLeftValuePoint.X / valuePerAcutalPixelX * -1;

            xAxisLine.X2 = args.ViewPixelSize.ActualPixelSize.X;
            yAxisLine.Y2 = args.ViewPixelSize.ActualPixelSize.Y;
        }

        private void SetXAxisStrokes(ViewArgs args)
        {
            int xAxisStrokeCount;
            float actualPixelWidth, valueWidth, valueDistanceBetweenStrokes, min, max;

            actualPixelWidth = args.ViewPixelSize.ActualPixelSize.X;
            valueWidth = args.ViewDimensions.ViewValueSize.X;
            valueDistanceBetweenStrokes = GetValueDistanceBetweenStrokes(actualPixelWidth, valueWidth);

            min = args.ViewDimensions.TopLeftValuePoint.X -
                (args.OverRender - 1) / 2F * args.ViewDimensions.ViewValueSize.X;
            max = args.ViewDimensions.BottomRightValuePoint.X +
                (args.OverRender - 1) / 2F * args.ViewDimensions.ViewValueSize.X;

            List<float> multiples = GetMultipleFromValuesInView(valueDistanceBetweenStrokes, min, max);

            for (xAxisStrokeCount = 0; xAxisStrokeCount < multiples.Count; xAxisStrokeCount++)
            {
                float x, y1, y2;

                x = GetAcutalPixelValue(multiples[xAxisStrokeCount], yAxisLine.X,
                    args.ViewDimensions.ViewValueSize.X / args.ViewPixelSize.ActualPixelSize.X);
                y1 = xAxisLine.Y - strokeLenght / 2;
                y2 = xAxisLine.Y + strokeLenght / 2;

                SetXAxisStroke(xAxisStrokeCount, x, y1, y2, multiples[xAxisStrokeCount]);
            }

            while (xAxisStrokes.Count > xAxisStrokeCount) xAxisStrokes.RemoveAt(xAxisStrokeCount);
        }

        private void SetYAxisStrokes(ViewArgs args)
        {
            int yAxisStrokeCount;
            float actualPixelHeight, valueHeight, valueDistanceBetweenStrokes, min, max;

            actualPixelHeight = args.ViewPixelSize.ActualPixelSize.Y;
            valueHeight = args.ViewDimensions.ViewValueSize.Y;
            valueDistanceBetweenStrokes = GetValueDistanceBetweenStrokes(actualPixelHeight, valueHeight);

            min = args.ViewDimensions.TopLeftValuePoint.Y -
              (args.OverRender - 1) / 2F * args.ViewDimensions.ViewValueSize.Y;
            max = args.ViewDimensions.BottomRightValuePoint.Y +
                (args.OverRender - 1) / 2F * args.ViewDimensions.ViewValueSize.Y;

            List<float> multiples = GetMultipleFromValuesInView(valueDistanceBetweenStrokes, min, max);

            for (yAxisStrokeCount = 0; yAxisStrokeCount < multiples.Count; yAxisStrokeCount++)
            {
                float x1, x2, y;

                x1 = yAxisLine.X - strokeLenght / 2;
                x2 = yAxisLine.X + strokeLenght / 2;
                y = GetAcutalPixelValue(multiples[yAxisStrokeCount] * -1, xAxisLine.Y,
                    -1 * args.ViewDimensions.ViewValueSize.Y / args.ViewPixelSize.ActualPixelSize.Y);

                SetYAxisStroke(yAxisStrokeCount, x1, x2, y, multiples[yAxisStrokeCount] * -1);
            }

            while (yAxisStrokes.Count > yAxisStrokeCount) yAxisStrokes.RemoveAt(yAxisStrokeCount);
        }

        private float GetValueDistanceBetweenStrokes(double pixelWidthOrHeight, float valueWidthOrHeight)
        {
            double aboutNumberOfStrokes = GetAboutNumberOfStrokes(pixelWidthOrHeight);
            double aboutValueDistanceBetweenStrokes = valueWidthOrHeight / aboutNumberOfStrokes;
            double factor = GetFactor(aboutValueDistanceBetweenStrokes);
            double minMultiAbs = 10, minMultiAbsDistance = 1;

            foreach (float distance in possibleDistanceBetweenStrokes)
            {
                if (!SetMinMultiAbsIfSmaller(ref minMultiAbs, ref minMultiAbsDistance,
                     aboutValueDistanceBetweenStrokes, distance * factor))
                {
                    return (float)minMultiAbsDistance;
                }
            }

            SetMinMultiAbsIfSmaller(ref minMultiAbs, ref minMultiAbsDistance,
                aboutValueDistanceBetweenStrokes, possibleDistanceBetweenStrokes[0] * 10 * factor);

            return (float)minMultiAbsDistance;
        }

        private double GetAboutNumberOfStrokes(double pixelWidthOrHeight)
        {
            return Math.Sqrt(pixelWidthOrHeight) / 2.5;
        }

        private double GetFactor(double aboutValueDistanceBetweenStrokes)
        {
            if (aboutValueDistanceBetweenStrokes <= 0) { }

            if (aboutValueDistanceBetweenStrokes < 1)
            {
                return GetFactorIfDistanceIsLow(aboutValueDistanceBetweenStrokes);
            }

            return GetFactorIfDistanceIsHigh(aboutValueDistanceBetweenStrokes);
        }

        private double GetFactorIfDistanceIsHigh(double aboutValueDistanceBetweenStrokes)
        {
            float possibleDistance = possibleDistanceBetweenStrokes[0];
            double currentfactor = 1, stepFactor = 10;

            while (true)
            {
                if (aboutValueDistanceBetweenStrokes < possibleDistance * currentfactor * stepFactor)
                {
                    return currentfactor;
                }

                currentfactor *= stepFactor;
            }
        }

        private double GetFactorIfDistanceIsLow(double aboutValueDistanceBetweenStrokes)
        {
            float possibleDistance = possibleDistanceBetweenStrokes[0];
            double currentfactor = 1, stepFactor = 0.1;

            while (true)
            {
                currentfactor *= stepFactor;

                if (aboutValueDistanceBetweenStrokes > possibleDistance * currentfactor)
                {
                    return currentfactor;
                }
            }
        }

        private bool SetMinMultiAbsIfSmaller(ref double minMultiAbs, ref double minMultiAbsDistance,
            double aboutValueDistanceBetweenStrokes, double distance)
        {
            double multiAbs = GetMultiAbs(aboutValueDistanceBetweenStrokes, distance);

            if (minMultiAbs > multiAbs)
            {
                minMultiAbs = multiAbs;
                minMultiAbsDistance = distance;

                return true;
            }

            return false;
        }

        private double GetMultiAbs(double aboutValueDistanceBetweenStrokes, double distance)
        {
            double multi = aboutValueDistanceBetweenStrokes / distance;

            if (multi < 1) multi = 1 / multi;

            return multi;
        }

        private List<float> GetMultipleFromValuesInView(float value, float min, float max)
        {
            List<float> multiples = new List<float>();
            long currentMultiple = Convert.ToInt64(Math.Floor(min / value));

            if (currentMultiple > 0) currentMultiple--;

            while (value * (currentMultiple - 1) <= max)
            {
                if (currentMultiple != 0)
                {
                    multiples.Add(value * currentMultiple);
                }

                currentMultiple++;
            }

            return multiples;
        }

        private float GetAcutalPixelValue(float value, float originAcutalPixelValue, float acutalPixelPerValue)
        {
            return originAcutalPixelValue + value / acutalPixelPerValue;
        }

        private void SetXAxisStroke(int index, float x, float y1, float y2, float value)
        {
            while (xAxisStrokes.Count <= index) xAxisStrokes.Add(new VerticalLineText());

            xAxisStrokes[index].X = x;
            xAxisStrokes[index].Y1 = y1;
            xAxisStrokes[index].Y2 = y2;
            xAxisStrokes[index].Value = value;
        }

        private void SetYAxisStroke(int index, float x1, float x2, float y, float value)
        {
            while (yAxisStrokes.Count <= index) yAxisStrokes.Add(new HorizontalLineText());

            yAxisStrokes[index].X1 = x1;
            yAxisStrokes[index].X2 = x2;
            yAxisStrokes[index].Y = y;
            yAxisStrokes[index].Value = value;
        }

        public void Draw(CanvasDrawingSession drawingSession, Vector2 actualSize)
        {
            ILineText preLineText = null;

            if (IsInView(xAxisLine, actualSize))
            {
                drawingSession.DrawLine(xAxisLine.Point1, xAxisLine.Point2, color, thickness);
            }

            if (IsInView(yAxisLine, actualSize))
            {
                drawingSession.DrawLine(yAxisLine.Point1, yAxisLine.Point2, color, thickness);
            }

            foreach (HorizontalLineText hlt in yAxisStrokes)
            {
                if (IsInView(hlt, preLineText, actualSize))
                {
                    drawingSession.DrawLine(hlt.Point1, hlt.Point2, color, thickness);
                    drawingSession.DrawText(hlt.ValueText, hlt.ValuePoint, color, hlt.Format);

                    preLineText = hlt;
                }
                else preLineText = null;
            }

            preLineText = null;

            foreach (VerticalLineText vlt in xAxisStrokes)
            {
                if (IsInView(vlt, preLineText, actualSize))
                {
                    drawingSession.DrawLine(vlt.Point1, vlt.Point2, color, thickness);
                    drawingSession.DrawText(vlt.ValueText, vlt.ValuePoint, color, vlt.Format);

                    preLineText = vlt;
                }
                else preLineText = null;
            }
        }

        private bool IsInView(HorizontalLine hl, Vector2 actualSize)
        {
            return !(hl.Point1.Y + thickness / 2 < 0 ||
                hl.Point1.Y - thickness / 2 > actualSize.Y);
        }

        private bool IsInView(VerticalLine vl, Vector2 actualSize)
        {
            return !(vl.Point1.X + thickness / 2 < 0 ||
                vl.Point1.X - thickness / 2 > actualSize.X);
        }

        private bool IsInView(ILineText curLine, ILineText preLine, Vector2 actualSize)
        {
            bool changedPosition = false;

            if (Overlap(curLine, preLine))
            {
                curLine.ChangePosition();
                changedPosition = true;
            }

            for (int i = 0; i < 2; i++)
            {
                Vector2 topLeft = curLine.TopLeftPoint, bottomRight = curLine.BottomRightPoint;

                if (topLeft.X > 0 && topLeft.Y > 0 && bottomRight.X < actualSize.X &&
                    bottomRight.Y < actualSize.Y) return true;

                if (!changedPosition) curLine.ChangePosition();
                else break;
            }

            for (int i = 0; i < 2; i++)
            {
                Vector2 topLeft = curLine.TopLeftPoint, bottomRight = curLine.BottomRightPoint;

                if ((topLeft.X > 0 && topLeft.Y > 0 && topLeft.X < actualSize.X && topLeft.Y < actualSize.Y) ||
                    (bottomRight.X > 0 && bottomRight.Y > 0 &&
                    bottomRight.X < actualSize.X && bottomRight.Y < actualSize.Y))
                {
                    return true;
                }

                if (!changedPosition) curLine.ChangePosition();
                else break;
            }

            return false;
        }

        private bool Overlap(ILineText line1, ILineText line2)
        {
            if (line1 == null || line2 == null) return false;

            if (!(IsBetween(line1.TopLeftPoint.X, line1.BottomRightPoint.X, line2.TopLeftPoint.X,
                line2.BottomRightPoint.X) || Enclose(line1.TopLeftPoint.X, line1.BottomRightPoint.X,
                line2.TopLeftPoint.X, line2.BottomRightPoint.X))) return false;

            return IsBetween(line1.TopLeftPoint.Y, line1.BottomRightPoint.Y, line2.TopLeftPoint.Y,
                line2.BottomRightPoint.Y) || Enclose(line1.TopLeftPoint.Y, line1.BottomRightPoint.Y,
                line2.TopLeftPoint.Y, line2.BottomRightPoint.Y);
        }

        private bool IsBetween(float min, float max, float value)
        {
            return min <= value && value < max;
        }

        private bool IsBetween(float min1, float max1, float min2, float max2)
        {
            return IsBetween(min1, max1, min2) || IsBetween(min2, max2, min1);
        }

        private bool Enclose(float min1, float max1, float min2, float max2)
        {
            return (max1 < min2 && max1 > max2) || (min2 < min1 && max2 > max1);
        }
    }
}