using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GraphomatDrawingLibUwp.ArrayManagement
{
    class PixelPoints : IEnumerable<Vector2>
    {
        private Vector2[] points;
        private ValuePoints valuePoints;

        public Vector2 this[int index] => points[index] + Offset;

        public Vector2 Offset { get; set; }

        public int Count => points.Length;

        public PixelPoints(ValuePoints valuePoints)
        {
            this.valuePoints = valuePoints;
        }

        public void Calculate(ViewArgs args)
        {
            points = new Vector2[valuePoints.Count];

            for (int i = 0; i < points.Length; i++)
            {
                points[i] = args.ToView(valuePoints[i]);
            }

            Offset = new Vector2();
        }

        public void Recalculate(ViewArgs args)
        {
            int pixelPointsLength, pixelPointsIndex = 0;
            float selectValueMin, valuesValueWidth, valuePerRawPixelX,
                selectValueMax, rawPixelXPerValues, rawPixelWidth, valueWidth;

            if (valuePoints.Count < 2) return;

            rawPixelWidth = args.PixelSize.RawPixelWidth;
            valueWidth = args.ValueDimensions.Width;

            valuesValueWidth = valuePoints[valuePoints.Count - 1].X - valuePoints[0].X;
            pixelPointsLength = Convert.ToInt32(rawPixelWidth * valuesValueWidth / valueWidth) + 1;
            points = new Vector2[pixelPointsLength];

            valuePerRawPixelX = valueWidth / rawPixelWidth;
            rawPixelXPerValues = valuesValueWidth / (valuePoints.Count - 1) / valuePerRawPixelX;

            selectValueMin = (1 - rawPixelXPerValues) / 2F * valuePerRawPixelX;
            selectValueMax = (1 + rawPixelXPerValues) / 2F * valuePerRawPixelX;

            for (int i = 0; i < valuePoints.Count && pixelPointsIndex < pixelPointsLength; i++)
            {
                Vector2 curValue = valuePoints[i];

                float nearAllowedValue = (curValue.X % valuePerRawPixelX + valuePerRawPixelX) % valuePerRawPixelX;

                if (selectValueMin <= nearAllowedValue && nearAllowedValue < selectValueMax)
                {
                    points[pixelPointsIndex] = args.ToView(curValue);

                    pixelPointsIndex++;
                }
            }

            Offset = new Vector2();
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
