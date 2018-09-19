using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    struct ViewValueDimensions
    {
        public float Width { get; private set; }

        public float Height { get; private set; }

        public Vector2 Size { get; private set; }

        public Vector2 Middle { get; private set; }

        public float Left { get; private set; }

        public float Top { get; private set; }

        public Vector2 TopLeft { get; private set; }

        public float Right { get; private set; }

        public float Bottom { get; private set; }

        public Vector2 BottomRight { get; private set; }

        public ViewValueDimensions(Vector2 valueSize, Vector2 middleOfViewValue) :
            this(valueSize.X, valueSize.Y, middleOfViewValue.X, middleOfViewValue.Y)
        {
        }

        public ViewValueDimensions(double viewValueWidth, double viewValueHeight, double middleOfViewValueX,
            double middleOfViewValueY) : this((float)viewValueWidth, (float)viewValueHeight,
                (float)middleOfViewValueX, (float)middleOfViewValueY)
        {
        }

        public ViewValueDimensions(float viewValueWidth, float viewValueHeight, float middleOfViewValueX,
            float middleOfViewValueY)
        {
            Width = viewValueWidth;
            Height = viewValueHeight;
            Size = new Vector2(viewValueWidth, viewValueHeight);

            Middle = new Vector2(middleOfViewValueX, middleOfViewValueY);

            Left = middleOfViewValueX - viewValueWidth / 2;
            Top = middleOfViewValueY - viewValueHeight / 2;
            TopLeft = new Vector2(Left, Top);

            Right = middleOfViewValueX + viewValueWidth / 2;
            Bottom = middleOfViewValueY + viewValueHeight / 2;
            BottomRight = new Vector2(Right, Bottom);
        }
    }
}
