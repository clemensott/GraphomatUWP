using System;
using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    struct ViewArgs : IEquatable<ViewArgs>
    {
        public static float BufferFactor => DrawControl.PixelBufferFactor;

        public ViewValueDimensions ValueDimensions { get; }

        public ViewPixelSize PixelSize { get; }

        public ViewArgs(ViewValueDimensions valueDimensions, ViewPixelSize pixelSize)
        {
            ValueDimensions = valueDimensions;
            PixelSize = pixelSize;
        }

        public float ToViewX(float value)
        {
            return (value - ValueDimensions.Left) * PixelSize.ActualWidth / ValueDimensions.Width;
        }

        public float ToViewY(float value)
        {
            return (ValueDimensions.Bottom - value) * PixelSize.ActualHeight / -ValueDimensions.Height;
        }

        public Vector2 ToView(Vector2 value)
        {
            return new Vector2(ToViewX(value.X), ToViewY(value.Y));
        }

        public override bool Equals(object obj)
        {
            return obj is ViewArgs && Equals((ViewArgs)obj);
        }

        public bool Equals(ViewArgs other)
        {
            return EqualityComparer<ViewValueDimensions>.Default.Equals(ValueDimensions, other.ValueDimensions) &&
                   PixelSize.Equals(other.PixelSize);
        }

        public override int GetHashCode()
        {
            var hashCode = -1115000291;
            hashCode = hashCode * -1521134295 + BufferFactor.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<ViewValueDimensions>.Default.GetHashCode(ValueDimensions);
            hashCode = hashCode * -1521134295 + EqualityComparer<ViewPixelSize>.Default.GetHashCode(PixelSize);
            return hashCode;
        }

        public static bool operator ==(ViewArgs args1, ViewArgs args2)
        {
            return args1.Equals(args2);
        }

        public static bool operator !=(ViewArgs args1, ViewArgs args2)
        {
            return !(args1 == args2);
        }
    }
}
