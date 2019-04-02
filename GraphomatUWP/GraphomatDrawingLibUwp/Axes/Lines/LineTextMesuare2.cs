using Windows.Foundation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace GraphomatDrawingLibUwp
{
    class LineTextMeasure2
    {
        private static readonly TextBlock tblSize = new TextBlock() { FontFamily = new FontFamily("Arial") };

        public static void Measure(float value, float valueSize, out float width, out float height)
        {
            tblSize.Text = value.ToString();
            tblSize.FontSize = valueSize;
            tblSize.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));

            width = (float)tblSize.DesiredSize.Width;
            height = (float)tblSize.DesiredSize.Height;
        }
    }
}
