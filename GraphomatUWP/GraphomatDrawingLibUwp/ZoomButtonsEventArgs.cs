using System;

namespace GraphomatDrawingLibUwp
{
    public  class ZoomButtonsEventArgs : EventArgs
    {
        public float ZoomFactorWidth { get; private set; }

        public float ZoomFactorHeight { get; private set; }

        public ZoomButtonsEventArgs(float zoomFactorWidth, float zoomFactorHeight)
        {
            ZoomFactorWidth = zoomFactorWidth;
            ZoomFactorHeight = zoomFactorHeight;
        }

        internal ViewDimensions GetChangedViewDimensions(ViewDimensions viewDimensions)
        {
            double viewValueWidth = viewDimensions.ViewValueSize.X / ZoomFactorWidth, 
                viewValueHeight = viewDimensions.ViewValueSize.Y / ZoomFactorHeight,
                middleOfViewValueX = viewDimensions.MiddleOfViewValuePoint.X,
                middleOfViewValueY = viewDimensions.MiddleOfViewValuePoint.Y;

            return new ViewDimensions(viewValueWidth, viewValueHeight, middleOfViewValueX, middleOfViewValueY);
        }
    }
}
