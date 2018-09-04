using Microsoft.Graphics.Canvas.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace GraphomatDrawingLibUwp
{
    class VerticalLineText : VerticalLine, ILineText
    {
        public const float TextSize = 10;

        private float value, valueTextWidth, valueTextHeight;
        private IVerticalLineText iVerticalLineText;

        public float Value
        {
            get { return value; }
            set { SetValuePoint(value); }
        }

        public string ValueText { get { return value.ToString(); } }

        public VerticalPostition PositionMode
        {
            get { return iVerticalLineText.PositionMode; }
            set { if (value != PositionMode) ChangePosition(); }
        }

        public Vector2 ValuePoint
        {
            get
            {
                return iVerticalLineText.GetValuePoint(X, Y1, Y2, valueTextWidth, valueTextHeight);
            }
        }

        public Vector2 TopLeftPoint
        {
            get
            {
                return iVerticalLineText.GetTopLeftPoint(X, Y1, Y2, valueTextWidth, valueTextHeight);
            }
        }

        public Vector2 BottomRightPoint
        {
            get
            {
                return iVerticalLineText.GetBottomRightPoint(X, Y1, Y2, valueTextWidth, valueTextHeight);
            }
        }

        public CanvasTextFormat Format { get; private set; }

        public VerticalLineText() : base()
        {
            Format = new CanvasTextFormat() { FontFamily = "Arial", FontSize = TextSize };
            SetValuePoint(0);
        }

        public VerticalLineText(float x, float y1, float y2, float value) : base(x, y1, y2)
        {
            Format = new CanvasTextFormat() { FontFamily = "Arial", FontSize = TextSize };
            SetValuePoint(value);
        }

        private void SetValuePoint(float value)
        {
            LineTextMeasure.Measure(value, TextSize, out valueTextWidth, out valueTextHeight);
            this.value = value;
            iVerticalLineText = new VerticalLineTextUnder();
        }

        public void ChangePosition()
        {
            iVerticalLineText = iVerticalLineText.OtherVerticalLineText;
        }
    }
}
