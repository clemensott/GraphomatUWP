namespace GraphomatDrawingLibUwp
{
    class ViewArgs
    {
        public float BufferFactor { get { return DrawControl.PixelBufferFactor; } }

        public ViewDimensions ViewDimensions { get; private set; }

        public ViewPixelSize ViewPixelSize { get; private set; }

        public ViewArgs(ViewDimensions viewDimensions, ViewPixelSize viewPixelSize)
        {
            ViewDimensions = viewDimensions;
            ViewPixelSize = viewPixelSize;
        }
    }
}
