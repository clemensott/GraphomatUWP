using System;
using System.Collections.Generic;

namespace GraphomatDrawingLibUwp
{
    struct ViewArgs : IEquatable<ViewArgs>
    {
        public float BufferFactor { get { return DrawControl.PixelBufferFactor; } }

        public ViewValueDimensions ValueDimensions { get; private set; }

        public ViewPixelSize PixelSize { get; private set; }

        public ViewArgs(ViewValueDimensions valueDimensions, ViewPixelSize pixelSize)
        {
            ValueDimensions = valueDimensions;
            PixelSize = pixelSize;
        }

        public override bool Equals(object obj)
        {
            return obj is ViewArgs && Equals((ViewArgs)obj);
        }

        public bool Equals(ViewArgs other)
        {
            return BufferFactor == other.BufferFactor &&
                   EqualityComparer<ViewValueDimensions>.Default.Equals(ValueDimensions, other.ValueDimensions) &&
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
