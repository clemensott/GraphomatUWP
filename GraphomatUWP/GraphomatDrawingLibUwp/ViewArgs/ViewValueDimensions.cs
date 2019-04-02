using System;
using System.Collections.Generic;
using System.Numerics;

namespace GraphomatDrawingLibUwp
{
    struct ViewValueDimensions : IEquatable<ViewValueDimensions>
    {
        public float Width { get; }

        public float Height { get; }

        public Vector2 Size { get; }

        public Vector2 Middle { get; }

        public float Left { get; }

        public float Top { get; }

        public Vector2 TopLeft { get; }

        public float Right { get; }

        public float Bottom { get; }

        public Vector2 BottomRight { get; }

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
            Top = middleOfViewValueY + viewValueHeight / 2;
            TopLeft = new Vector2(Left, Top);

            Right = middleOfViewValueX + viewValueWidth / 2;
            Bottom = middleOfViewValueY - viewValueHeight / 2;
            BottomRight = new Vector2(Right, Bottom);
        }

        public override bool Equals(object obj)
        {
            return obj is ViewValueDimensions && Equals((ViewValueDimensions)obj);
        }

        public bool Equals(ViewValueDimensions other)
        {
            return Width == other.Width &&
                   Height == other.Height &&
                   Middle.Equals(other.Middle) &&
                   Left == other.Left &&
                   Top == other.Top &&
                   Right == other.Right &&
                   Bottom == other.Bottom;
        }

        public override int GetHashCode()
        {
            var hashCode = -1871709828;
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Vector2>.Default.GetHashCode(Middle);
            hashCode = hashCode * -1521134295 + Left.GetHashCode();
            hashCode = hashCode * -1521134295 + Top.GetHashCode();
            hashCode = hashCode * -1521134295 + Right.GetHashCode();
            hashCode = hashCode * -1521134295 + Bottom.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(ViewValueDimensions dims1, ViewValueDimensions dims2)
        {
            return dims1.Equals(dims2);
        }

        public static bool operator !=(ViewValueDimensions dims1, ViewValueDimensions dims2)
        {
            return !(dims1 == dims2);
        }
    }
}
