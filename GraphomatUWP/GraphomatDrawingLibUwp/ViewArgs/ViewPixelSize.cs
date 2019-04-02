using System;
using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    struct ViewPixelSize : IEquatable<ViewPixelSize>
    {
        public float RawPixelWidth { get; }

        public float ActualWidth { get; }

        public float ActualHeight { get; }

        public Vector2 ActualPixelSize { get; }

        public ViewPixelSize(double actualPixelWidth, double actualPixelHeight, double rawPixelPerActualPixel)
        {
            ActualWidth = (float)actualPixelWidth;
            ActualHeight = (float)actualPixelHeight;
            ActualPixelSize = new Vector2(ActualWidth, ActualHeight);
            RawPixelWidth = (float)(actualPixelWidth * rawPixelPerActualPixel);
        }

        public override bool Equals(object obj)
        {
            return obj is ViewPixelSize && Equals((ViewPixelSize)obj);
        }

        public bool Equals(ViewPixelSize other)
        {
            return RawPixelWidth == other.RawPixelWidth &&
                   ActualWidth == other.ActualWidth &&
                   ActualHeight == other.ActualHeight;
        }

        public override int GetHashCode()
        {
            var hashCode = -1295356442;
            hashCode = hashCode * -1521134295 + RawPixelWidth.GetHashCode();
            hashCode = hashCode * -1521134295 + ActualWidth.GetHashCode();
            hashCode = hashCode * -1521134295 + ActualHeight.GetHashCode();
            return hashCode;
        }
    }
}
