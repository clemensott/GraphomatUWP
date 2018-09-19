using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatDrawingLibUwp
{
    class PixelPoints : IEnumerable<Vector2>
    {
        private Vector2[] points;
        private ValuePoints valuePoints;

        public Vector2 this[int index] { get { return points[index] + Offset; } }

        public Vector2 Offset { get; set; }

        public int Count { get { return points.Length; } }

        public PixelPoints(ValuePoints valuePoints)
        {
            this.valuePoints = valuePoints;
        }

        public void Calculate(ViewArgs args)
        {
            float valuePerActualPixelY, originFromTopY, actualPixelXPerValue, leftSideX;

            points = new Vector2[valuePoints.Count];

            valuePerActualPixelY = args.ValueDimensions.Height / args.PixelSize.Height;
            originFromTopY = args.ValueDimensions.Top / valuePerActualPixelY * -1;

            actualPixelXPerValue = args.PixelSize.ActualPixelSize.X / args.ValueDimensions.Width;
            leftSideX = args.ValueDimensions.Left;

            for (int i = 0; i < points.Length; i++)
            {
                Vector2 curValue = valuePoints[i];
                float curPixelPointX = (curValue.X - leftSideX) * actualPixelXPerValue;
                float curPixelPointY = originFromTopY - curValue.Y / valuePerActualPixelY;

                points[i] = new Vector2(curPixelPointX, curPixelPointY);
            }

            Offset = new Vector2();
        }

        public void Recalculate(ViewArgs args)
        {
            int pixelPointsLength, pixelPointsIndex = 0;
            float valuePerActualPixelY, originFromTopY, selectValueMin, valuesValueWidth, valuePerRawPixelX,
                selectValueMax, rawPixelXPerValues, actualPixelXPerValue, leftSideX, rawPixelWidth, valueWidth;

            if (valuePoints.Count < 2) return;

            rawPixelWidth = args.PixelSize.RawPixelWidth;
            valueWidth = args.ValueDimensions.Width;

            valuePerActualPixelY = args.ValueDimensions.Height / args.PixelSize.Height;
            originFromTopY = args.ValueDimensions.Top / valuePerActualPixelY * -1;

            valuesValueWidth = valuePoints[valuePoints.Count - 1].X - valuePoints[0].X;
            pixelPointsLength = Convert.ToInt32(rawPixelWidth * valuesValueWidth / valueWidth) + 1;
            points = new Vector2[pixelPointsLength];

            valuePerRawPixelX = valueWidth / rawPixelWidth;
            rawPixelXPerValues = valuesValueWidth / (valuePoints.Count - 1) / valuePerRawPixelX;

            selectValueMin = (1 - rawPixelXPerValues) / 2F * valuePerRawPixelX;
            selectValueMax = (1 + rawPixelXPerValues) / 2F * valuePerRawPixelX;

            actualPixelXPerValue = args.PixelSize.ActualPixelSize.X / valueWidth;
            leftSideX = args.ValueDimensions.Left;

            for (int i = 0; i < valuePoints.Count && pixelPointsIndex < pixelPointsLength; i++)
            {
                Vector2 curValue = valuePoints[i];

                float nearAllowedValue = (curValue.X % valuePerRawPixelX + valuePerRawPixelX) % valuePerRawPixelX;

                if (selectValueMin <= nearAllowedValue && nearAllowedValue < selectValueMax)
                {
                    float currentPixelPointY = originFromTopY - curValue.Y / valuePerActualPixelY;
                    float currentPixelPointX = (curValue.X - leftSideX) * actualPixelXPerValue;

                    points[pixelPointsIndex] = new Vector2(currentPixelPointX, currentPixelPointY);

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
