using Microsoft.Graphics.Canvas.Text;
using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    class HorizontalLineText : HorizontalLine,ILineText
    {
        public const float TextSize = 10;

        private float value, valueTextWidth, valueTextHeight;
        private IHorizontalLineText ihorizontalLineText;

        public float Value
        {
            get => value;
            set => SetValuePoint(value);
        }

        public string ValueText => value.ToString();

        public HorizontalPosition PositionMode
        {
            get => ihorizontalLineText.PositionMode;
            set { if (value != PositionMode) ChangePosition(); }
        }

        public Vector2 ValuePoint => ihorizontalLineText.GetValuePoint(X1, X2, Y, valueTextWidth, valueTextHeight);

        public Vector2 TopLeftPoint => ihorizontalLineText.GetTopLeftPoint(X1, X2, Y, valueTextWidth, valueTextHeight);

        public Vector2 BottomRightPoint => ihorizontalLineText.GetBottomRightPoint(X1, X2, Y, valueTextWidth, valueTextHeight);

        public CanvasTextFormat Format { get; }

        public HorizontalLineText() : base()
        {
            Format = new CanvasTextFormat() { FontFamily = "Arial", FontSize = TextSize };
            SetValuePoint(0);
        }

        public HorizontalLineText(float x1, float x2, float y, float value) : base(x1, x2, y)
        {
            Format = new CanvasTextFormat() { FontFamily = "Arial", FontSize = TextSize };
            SetValuePoint(value);
        }

        private void SetValuePoint(float value)
        {
            LineTextMeasure2.Measure(value, TextSize, out valueTextWidth, out valueTextHeight);
            this.value = value;
            ihorizontalLineText = new HorizontalLineTextLeft();
        }

        public void ChangePosition()
        {
            ihorizontalLineText = ihorizontalLineText.OtherIHorizontalLineText;
        }
    }
}
