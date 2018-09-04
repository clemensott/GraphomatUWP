using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    class ViewDimensions
    {
        public Vector2 ViewValueSize { get; private set; }

        public Vector2 MiddleOfViewValuePoint { get; private set; }

        public Vector2 TopLeftValuePoint { get; private set; }

        public Vector2 BottomRightValuePoint { get; private set; }

        public ViewDimensions(Vector2 valueSize, Vector2 middleOfViewValue) :
            this(valueSize.X, valueSize.Y, middleOfViewValue.X, middleOfViewValue.Y)
        {
        }

        public ViewDimensions(double viewValueWidth, double viewValueHeight, double middleOfViewValueX,
            double middleOfViewValueY) : this((float)viewValueWidth, (float)viewValueHeight, 
                (float)middleOfViewValueX, (float)middleOfViewValueY)
        {
        }

        public ViewDimensions(float viewValueWidth, float viewValueHeight, float middleOfViewValueX,
            float middleOfViewValueY)
        {
            ViewValueSize = new Vector2(viewValueWidth, viewValueHeight);
            MiddleOfViewValuePoint = new Vector2(middleOfViewValueX, middleOfViewValueY);

            TopLeftValuePoint = new Vector2(middleOfViewValueX - viewValueWidth / 2,
                middleOfViewValueY - viewValueHeight / 2);
            BottomRightValuePoint = new Vector2(middleOfViewValueX + viewValueWidth / 2,
                middleOfViewValueY + viewValueHeight / 2);
        }

        public ViewDimensions Clone()
        {
            return new ViewDimensions(ViewValueSize.X, ViewValueSize.Y, MiddleOfViewValuePoint.X,
                MiddleOfViewValuePoint.Y);
        }
    }
}
