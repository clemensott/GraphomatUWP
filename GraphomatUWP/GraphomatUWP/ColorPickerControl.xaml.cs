using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GraphomatUWP
{
    public sealed partial class ColorPickerControl : UserControl
    {
        private const int rectWidthAndHeight = 30, rectMargin = 5;

        private IReadOnlyList<Color> allColors;
        private Color[,] analogColors;

        public ColorPickerControl()
        {
            this.InitializeComponent();

            allColors = GetKnownColors();
            analogColors = GetAnalogColors(10);
        }

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //analogColors = GetAnalogColors(ccColors.ActualWidth);

           // ccColors.Invalidate();
        }

        private void DrawColors(CanvasControl sender, CanvasDrawEventArgs args)
        {
            int analogColorsWidth = analogColors.GetLength(0), analogColorsHeight = analogColors.GetLength(1);
            Rect rect1 = new Rect(0, 0, 2, 2);

            for (int i = 0; i < analogColorsWidth; i++)
            {
                for (int j = 0; j < analogColorsHeight; j++)
                {
                    args.DrawingSession.FillRectangle(rect1, analogColors[i, j]);
                    rect1.Y++;
                }

                rect1.X++;
                rect1.Y = 0;
            }

            //return;
            Rect rect = new Rect(0, 0, rectWidthAndHeight, rectWidthAndHeight);
            int columns = Convert.ToInt32((sender.ActualWidth + rectMargin) / (rectWidthAndHeight + rectMargin));
            int rows = allColors.Count / columns + (allColors.Count % columns > 0 ? 1 : 0);

            sender.Height = rows * rectWidthAndHeight + (rows - 1) * rectMargin;

            for (int i = 0; i < allColors.Count; i++)
            {
                int column = i % columns;
                int row = i / columns;

                rect.X = column * (rectWidthAndHeight + rectMargin);
                rect.Y = row * (rectWidthAndHeight + rectMargin);

                args.DrawingSession.FillRectangle(rect, allColors[i]);
            }
        }

        private Color[,] GetAnalogColors(double width)
        {
            //     0              256           512           768           1024            1280          1536
            //r255 g0 b0 -> r255 g255 b0 -> r0 g255 b0 -> r0 g255 b255 -> r0 g0 b255 -> r255 g0 b255 -> r255 g0 b0

            int widthInt = Convert.ToInt32(width), heightInt = 512;
            Color[,] analogColors = new Color[widthInt, heightInt];

            for (int i = 0; i < widthInt; i++)
            {
                int x = Convert.ToInt32(Convert.ToDouble(i) / widthInt * 1536);

                for (int j = 0; j < heightInt; j++)
                {
                    if (j == 256 || j == 511) { }
                    analogColors[i, j] = GetColorWithY(j, GetColorWithX(x));
                }
            }

            return analogColors;
        }

        private Color GetColorWithX(int x)
        {
            byte r = 0, g = 0, b = 0;

            if (x > 1024 || x < 512) r = (byte)(256 - Math.Abs(x < 512 ? x : x - 1536) / 2);
            if (x > 0 && x < 1024) g = (byte)(256 - Math.Abs(x - 512) / 2);
            if (x > 512 && x < 1536) b = (byte)(256 - Math.Abs(x - 1024) / 2);

            return Color.FromArgb(255, (byte)(r / 2), (byte)(g / 2), (byte)(b / 2));
        }

        private Color GetColorWithY(int y, Color xColor)
        {
            if (y <= 256)
            {
                double factor = y / 256.0;

                xColor.R = Convert.ToByte(xColor.R * factor);
                xColor.G = Convert.ToByte(xColor.G * factor);
                xColor.B = Convert.ToByte(xColor.B * factor);
            }
            else
            {
                double factor = (y - 256) / 256.0;

                xColor.R = Convert.ToByte((255 - xColor.R) * factor + xColor.R);
                xColor.G = Convert.ToByte((255 - xColor.G) * factor + xColor.G);
                xColor.B = Convert.ToByte((255 - xColor.B) * factor + xColor.B);
            }

            return xColor;
        }

        private IReadOnlyList<Color> GetKnownColors()
        {
            List<Color> colors = new List<Color>();

            colors.Add(Colors.AliceBlue);
            colors.Add(Colors.AntiqueWhite);
            colors.Add(Colors.Aqua);
            colors.Add(Colors.Aquamarine);
            colors.Add(Colors.Azure);
            colors.Add(Colors.Beige);
            colors.Add(Colors.Bisque);
            colors.Add(Colors.Black);
            colors.Add(Colors.BlanchedAlmond);
            colors.Add(Colors.Blue);
            colors.Add(Colors.BlueViolet);
            colors.Add(Colors.Brown);
            colors.Add(Colors.BurlyWood);
            colors.Add(Colors.CadetBlue);
            colors.Add(Colors.Chartreuse);
            colors.Add(Colors.Chocolate);
            colors.Add(Colors.Coral);
            colors.Add(Colors.CornflowerBlue);
            colors.Add(Colors.Cornsilk);
            colors.Add(Colors.Crimson);
            colors.Add(Colors.Cyan);
            colors.Add(Colors.DarkBlue);
            colors.Add(Colors.DarkCyan);
            colors.Add(Colors.DarkGoldenrod);
            colors.Add(Colors.DarkGray);
            colors.Add(Colors.DarkGreen);
            colors.Add(Colors.DarkKhaki);
            colors.Add(Colors.DarkMagenta);
            colors.Add(Colors.DarkOliveGreen);
            colors.Add(Colors.DarkOrange);
            colors.Add(Colors.DarkOrchid);
            colors.Add(Colors.DarkRed);
            colors.Add(Colors.DarkSalmon);
            colors.Add(Colors.DarkSeaGreen);
            colors.Add(Colors.DarkSlateBlue);
            colors.Add(Colors.DarkSlateGray);
            colors.Add(Colors.DarkTurquoise);
            colors.Add(Colors.DarkViolet);
            colors.Add(Colors.DeepPink);
            colors.Add(Colors.DeepSkyBlue);
            colors.Add(Colors.DimGray);
            colors.Add(Colors.DodgerBlue);
            colors.Add(Colors.Firebrick);
            colors.Add(Colors.FloralWhite);
            colors.Add(Colors.ForestGreen);
            colors.Add(Colors.Fuchsia);
            colors.Add(Colors.Gainsboro);
            colors.Add(Colors.GhostWhite);
            colors.Add(Colors.Gold);
            colors.Add(Colors.Goldenrod);
            colors.Add(Colors.Gray);
            colors.Add(Colors.Green);
            colors.Add(Colors.GreenYellow);
            colors.Add(Colors.Honeydew);
            colors.Add(Colors.HotPink);
            colors.Add(Colors.IndianRed);
            colors.Add(Colors.Indigo);
            colors.Add(Colors.Ivory);
            colors.Add(Colors.Khaki);
            colors.Add(Colors.Lavender);
            colors.Add(Colors.LavenderBlush);
            colors.Add(Colors.LawnGreen);
            colors.Add(Colors.LemonChiffon);
            colors.Add(Colors.LightBlue);
            colors.Add(Colors.LightCoral);
            colors.Add(Colors.LightCyan);
            colors.Add(Colors.LightGoldenrodYellow);
            colors.Add(Colors.LightGray);
            colors.Add(Colors.LightGreen);
            colors.Add(Colors.LightPink);
            colors.Add(Colors.LightSalmon);
            colors.Add(Colors.LightSeaGreen);
            colors.Add(Colors.LightSkyBlue);
            colors.Add(Colors.LightSlateGray);
            colors.Add(Colors.LightSteelBlue);
            colors.Add(Colors.LightYellow);
            colors.Add(Colors.Lime);
            colors.Add(Colors.LimeGreen);
            colors.Add(Colors.Linen);
            colors.Add(Colors.Magenta);
            colors.Add(Colors.Maroon);
            colors.Add(Colors.MediumAquamarine);
            colors.Add(Colors.MediumBlue);
            colors.Add(Colors.MediumOrchid);
            colors.Add(Colors.MediumPurple);
            colors.Add(Colors.MediumSeaGreen);
            colors.Add(Colors.MediumSlateBlue);
            colors.Add(Colors.MediumSpringGreen);
            colors.Add(Colors.MediumTurquoise);
            colors.Add(Colors.MediumVioletRed);
            colors.Add(Colors.MidnightBlue);
            colors.Add(Colors.MintCream);
            colors.Add(Colors.MistyRose);
            colors.Add(Colors.Moccasin);
            colors.Add(Colors.NavajoWhite);
            colors.Add(Colors.Navy);
            colors.Add(Colors.OldLace);
            colors.Add(Colors.Olive);
            colors.Add(Colors.OliveDrab);
            colors.Add(Colors.Orange);
            colors.Add(Colors.OrangeRed);
            colors.Add(Colors.Orchid);
            colors.Add(Colors.PaleGoldenrod);
            colors.Add(Colors.PaleGreen);
            colors.Add(Colors.PaleTurquoise);
            colors.Add(Colors.PaleVioletRed);
            colors.Add(Colors.PapayaWhip);
            colors.Add(Colors.PeachPuff);
            colors.Add(Colors.Peru);
            colors.Add(Colors.Pink);
            colors.Add(Colors.Plum);
            colors.Add(Colors.PowderBlue);
            colors.Add(Colors.Purple);
            colors.Add(Colors.Red);
            colors.Add(Colors.Red);
            colors.Add(Colors.RosyBrown);
            colors.Add(Colors.RoyalBlue);
            colors.Add(Colors.SaddleBrown);
            colors.Add(Colors.Salmon);
            colors.Add(Colors.SandyBrown);
            colors.Add(Colors.SeaGreen);
            colors.Add(Colors.SeaShell);
            colors.Add(Colors.Sienna);
            colors.Add(Colors.Silver);
            colors.Add(Colors.SkyBlue);
            colors.Add(Colors.SlateBlue);
            colors.Add(Colors.SlateGray);
            colors.Add(Colors.Snow);
            colors.Add(Colors.SpringGreen);
            colors.Add(Colors.SteelBlue);
            colors.Add(Colors.Tan);
            colors.Add(Colors.Teal);
            colors.Add(Colors.Thistle);
            colors.Add(Colors.Tomato);
            colors.Add(Colors.Transparent);
            colors.Add(Colors.Turquoise);
            colors.Add(Colors.Violet);
            colors.Add(Colors.Wheat);
            colors.Add(Colors.White);
            colors.Add(Colors.WhiteSmoke);
            colors.Add(Colors.Yellow);
            colors.Add(Colors.YellowGreen);

            return colors.OrderBy(x => x.B).ThenBy(x => x.G).
                ThenBy(x => x.R).ToList().AsReadOnly();
        }
    }
}
