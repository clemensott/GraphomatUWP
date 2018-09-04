using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace GraphomatUWP
{
    public class ViewDimensions
    {
        public Vector2 ViewValueSize { get; private set; }

        public Vector2 MiddleOfViewValuePoint { get; private set; }

        public Vector2 TopLeftValuePoint { get; private set; }

        public Vector2 BottomRightValuePoint { get; private set; }

        public Vector2 ActualPixelSize { get; private set; }

        public ViewDimensions(double viewValueWidth, double viewValueHeight, double middleOfViewX, 
            double middleOfViewY) : this(Convert.ToSingle(viewValueWidth), Convert.ToSingle(viewValueHeight),
                  Convert.ToSingle(middleOfViewX), Convert.ToSingle(middleOfViewY))
        {
        }

        public ViewDimensions(float viewValueWidth, float viewValueHeight, 
            float middleOfViewX, float middleOfViewY)
        {
            ViewValueSize = new Vector2(viewValueWidth, viewValueHeight);
            MiddleOfViewValuePoint = new Vector2(middleOfViewX, middleOfViewY);

            TopLeftValuePoint = new Vector2(middleOfViewX - viewValueWidth / 2,
                middleOfViewY - viewValueHeight / 2);
            BottomRightValuePoint = new Vector2(middleOfViewX + viewValueWidth / 2,
                middleOfViewY + viewValueHeight / 2);

            Vector2 actualPixelSize = MoveScollManager.Current.ActualPixelSize;
            ActualPixelSize = new Vector2(actualPixelSize.X, actualPixelSize.Y);
        }

        public ViewDimensions Clone()
        {
            return new ViewDimensions(ViewValueSize.X, ViewValueSize.Y,
                MiddleOfViewValuePoint.X, MiddleOfViewValuePoint.Y);
        }
    }
}
