using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    struct ViewPixelSize
    {
        public float RawPixelWidth { get; private set; }

        public float Width { get; private set; }

        public float Height { get; private set; }

        public Vector2 ActualPixelSize { get; private set; }

        public ViewPixelSize(double actualPixelWidth, double actualPixelHeight, double rawPixelPerActualPixel)
        {
            Width = (float)actualPixelWidth;
            Height = (float)actualPixelHeight;
            ActualPixelSize = new Vector2(Width, Height);
            RawPixelWidth = (float)(actualPixelWidth * rawPixelPerActualPixel);
        }
    }
}
