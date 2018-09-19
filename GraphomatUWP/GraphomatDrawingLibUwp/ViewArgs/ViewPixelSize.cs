using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    struct ViewPixelSize
    {
        public float RawPixelWidth { get; private set; }

        public float ActualWidth { get; private set; }

        public float ActualHeight { get; private set; }

        public Vector2 ActualPixelSize { get; private set; }

        public ViewPixelSize(double actualPixelWidth, double actualPixelHeight, double rawPixelPerActualPixel)
        {
            ActualWidth = (float)actualPixelWidth;
            ActualHeight = (float)actualPixelHeight;
            ActualPixelSize = new Vector2(ActualWidth, ActualHeight);
            RawPixelWidth = (float)(actualPixelWidth * rawPixelPerActualPixel);
        }
    }
}
