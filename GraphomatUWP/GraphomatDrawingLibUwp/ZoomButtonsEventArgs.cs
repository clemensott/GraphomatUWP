using System;

namespace GraphomatDrawingLibUwp
{
    public class ZoomButtonsEventArgs : EventArgs
    {
        public float ZoomFactorWidth { get; }

        public float ZoomFactorHeight { get; }

        public ZoomButtonsEventArgs(float zoomFactorWidth, float zoomFactorHeight)
        {
            ZoomFactorWidth = zoomFactorWidth;
            ZoomFactorHeight = zoomFactorHeight;
        }

        internal ViewValueDimensions GetChangedValueDimensions(ViewValueDimensions valueDimensions)
        {
            double viewValueWidth = valueDimensions.Width / ZoomFactorWidth,
                viewValueHeight = valueDimensions.Height / ZoomFactorHeight,
                middleOfViewValueX = valueDimensions.Middle.X,
                middleOfViewValueY = valueDimensions.Middle.Y;

            return new ViewValueDimensions(viewValueWidth, viewValueHeight, middleOfViewValueX, middleOfViewValueY);
        }
    }
}
