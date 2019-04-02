namespace GraphomatDrawingLibUwp
{
    class LineTextMeasure
    {
        private const float refSize = 100, textHeight = 110, numberWidth = 63.73F, commaPointWidth = 35.87F,
            eWidth = 74.73F, plusWidth = 67, minusWidth = 41.4F, beforeAfterValueSpace = -3;

        public static void Measure(float value, float valueSize, out float width, out float height)
        {
            width = beforeAfterValueSpace;

            foreach (char c in value.ToString())
            {
                if (char.IsNumber(c)) width += numberWidth;
                else if (c == ',') width += commaPointWidth;
                else if (c == 'E') width += eWidth;
                else if (c == '+') width += plusWidth;
                else if (c == '-') width += minusWidth;
            }

            width += beforeAfterValueSpace;
            height = textHeight;

            width *= valueSize / refSize;
            height *= valueSize / refSize;
        }
    }
}
