namespace GraphomatDrawingLibUwp
{
    struct ViewArgs
    {
        public float BufferFactor { get { return DrawControl.PixelBufferFactor; } }

        public ViewValueDimensions ValueDimensions { get; private set; }

        public ViewPixelSize PixelSize { get; private set; }

        public ViewArgs(ViewValueDimensions valueDimensions, ViewPixelSize pixelSize)
        {
            ValueDimensions = valueDimensions;
            PixelSize = pixelSize;
        }
    }
}
