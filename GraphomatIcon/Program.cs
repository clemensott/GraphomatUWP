using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphomatIcon
{
    class Program
    {   // f = factor, dw/h = display width/height, ew/h = extra width/height
        const int f = 500, dw = 8, dh = 5, ew = 4, eh = 4;

        // bl/r/t/b = border left/right/top/bottom
        static int bl = 3, br = 2, bt = 4, bb = 4;

        static void Main(string[] args)
        {
            int[] dds = new int[] { 89, 107, 71, 142, 284, 188, 225, 150,
                300, 600, 388, 465, 310, 620, 1240, 55, 66, 44, 88, 176, 16,
                24, 48, 256, 50, 63, 75, 100, 200, 96, 48, 36, 30, 24 };

            int[] ds = new int[] { 388, 465, 310, 620, 1240, 775, 930, 2480 };
            int[] es = new int[] { 188, 225, 150, 300, 600, 375, 450, 1200 };

            Bitmap bmp = GetBasicBitmap();

            foreach (int d in dds)
            {
                AddSinusAndSave(bmp, d);
            }

            br = 1;
            bl = 0;
            bt = bb = 2;

            for (int i = 0; i < ds.Length; i++)
            {
                AddSinusAndSave(bmp, ds[i], es[i]);
            }
        }

        private static void AddSinusAndSave(Bitmap basicBmp, int d)
        {
            Bitmap bmpSinus = AddSinus(basicBmp, d, d);
            Bitmap bmpOut = new Bitmap(bmpSinus, d, d);

            string filename = string.Format("Icon{0}.png", d);

            bmpOut.Save(filename, ImageFormat.Png);

            bmpSinus.Dispose();
            bmpOut.Dispose();
        }

        private static void AddSinusAndSave(Bitmap basicBmp, int d, int e)
        {
            Bitmap bmpSinus = AddSinus(basicBmp, d, e);
            Bitmap bmpOut = new Bitmap(bmpSinus, d, e);

            string filename = string.Format("Icon{0}x{1}.png", d, e);

            bmpOut.Save(filename, ImageFormat.Png);

            bmpSinus.Dispose();
            bmpOut.Dispose();
        }

        private static Bitmap AddSinus(Bitmap basicBmp, int w, int h)
        {
            int fw = F(dw + ew + bl + br), fh = F(dh + eh + bt + bb);
            Bitmap bmp = new Bitmap(fw * w / h, fh);
            int offsetX = (bmp.Width - bmp.Height) / 2;

            using (Graphics g = Graphics.FromImage(bmp))
            {
                Point[] points = new Point[f * (dw + 1)];
                double k = Math.Sqrt(h);

                for (int i = 0; i < points.Length; i++)
                {
                    int x = F(bl) + offsetX + i + f / 2;
                    double sin = (dh - 1) / -2.0 * Math.Sin(i * k / 10 / f + 1);
                    int y = Convert.ToInt32(F(bt + 1 + dh / 2.0 + sin));

                    points[i] = new Point(x, y);
                }

                g.DrawImage(basicBmp, offsetX + F(bl), F(bt));
                g.DrawCurve(new Pen(Color.White, 0.15F * f), points);
            }

            Console.WriteLine("{0}x{1}", w, h);

            return bmp;
        }

        private static Bitmap GetBasicBitmap()
        {
            Brush brush = Brushes.White;
            Bitmap bmp = new Bitmap(F(dw + ew), F(dh + eh));

            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Line Above the display
                g.FillRectangle(brush, F(1), F(0), F(dw), F(1));

                // Lines left and right the display
                g.FillRectangle(brush, F(0), F(1), F(1), F(dh + 1));
                g.FillRectangle(brush, F(1 + dw), F(1), F(1), F(dh + 1));

                // Corner above left and right the display
                g.FillPie(brush, F(0), F(0), F(2), F(2), 180, 90);
                g.FillPie(brush, F(dw), F(0), F(2), F(2), 270, 90);

                // Rectangle under the display
                g.FillRectangle(brush, F(0), F(dh + 1), F(dw + 2), F(3));

                // Dongel on the right side
                g.FillRectangle(brush, F(dw + 2.5), F(dh + 1.5), F(1.5), F(2));
                g.FillRectangle(brush, F(dw + 3), F(dh - 1), F(0.5), F(2.5));
                g.FillEllipse(brush, F(dw + 2.5), F(dh - 2.25), F(1.5), F(1.5));

                // Axes in dhe display
                g.FillRectangle(brush, F(0.5), F(1 + dh / 2.0), F(dw + 1), F(0.1));
                g.FillRectangle(brush, F(1 + dw / 2.0), F(0.5), F(0.1), F(dh + 1));
            }

            return bmp;
        }

        private static int F(int value)
        {
            return value * f;
        }

        private static float F(double value)
        {
            return (float)(value * f);
        }
    }
}
