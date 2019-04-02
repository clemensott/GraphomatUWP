using Microsoft.Graphics.Canvas.Text;
using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    class VerticalLineText : VerticalLine, ILineText
    {
        public const float TextSize = 10;

        private float value, valueTextWidth, valueTextHeight;
        private IVerticalLineText iVerticalLineText;

        public float Value
        {
            get => value;
            set => SetValuePoint(value);
        }

        public string ValueText => value.ToString();

        public VerticalPostition PositionMode
        {
            get => iVerticalLineText.PositionMode;
            set { if (value != PositionMode) ChangePosition(); }
        }

        public Vector2 ValuePoint => iVerticalLineText.GetValuePoint(X, Y1, Y2, valueTextWidth, valueTextHeight);

        public Vector2 TopLeftPoint => iVerticalLineText.GetTopLeftPoint(X, Y1, Y2, valueTextWidth, valueTextHeight);

        public Vector2 BottomRightPoint => iVerticalLineText.GetBottomRightPoint(X, Y1, Y2, valueTextWidth, valueTextHeight);

        public CanvasTextFormat Format { get; }

        public VerticalLineText()
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
            LineTextMeasure2.Measure(value, TextSize, out valueTextWidth, out valueTextHeight);
            this.value = value;
            iVerticalLineText = new VerticalLineTextUnder();
        }

        public void ChangePosition()
        {
            iVerticalLineText = iVerticalLineText.OtherVerticalLineText;
        }
    }
}
