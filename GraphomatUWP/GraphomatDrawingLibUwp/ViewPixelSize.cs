using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    class ViewPixelSize
    {
        public float RawPixelWidth { get; private set; }

        public Vector2 ActualPixelSize { get; private set; }

        public ViewPixelSize(double actualPixelWidth, double actualPixelHeight, double rawPixelPerActualPixel)
        {
            ActualPixelSize = new Vector2((float)actualPixelWidth, (float)actualPixelHeight);
            RawPixelWidth = (float)(actualPixelWidth * rawPixelPerActualPixel);
        }
    }
}
