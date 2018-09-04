namespace GraphomatDrawingLibUwp
{
    class ViewArgs
    {
        public float OverRender { get { return DrawControl.MoreRenderFactor; } }

        public ViewDimensions ViewDimensions { get; private set; }

        public ViewPixelSize ViewPixelSize { get; private set; }

        public ViewArgs(ViewDimensions viewDimensions, ViewPixelSize viewPixelSize)
        {
            ViewDimensions = viewDimensions;
            ViewPixelSize = viewPixelSize;
        }
    }
}
