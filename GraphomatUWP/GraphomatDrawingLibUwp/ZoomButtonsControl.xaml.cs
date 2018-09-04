using Microsoft.Graphics.Canvas.UI.Xaml;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GraphomatDrawingLibUwp
{
    public delegate void ZoomedEventHandler(object sender, ZoomButtonsEventArgs args);

    enum ZoomProperty { In, Out, Stay }

    public sealed partial class ZoomButtonsControl : UserControl
    {
        public event ZoomedEventHandler Zoomed;
        private const int columnsAndRows = 3;
        private const float zoomFactorWidth = 1.5F, zoomFactorHeight = 1.5F,
            borderAroundCanvasPercent = 0.15F, lineThicknessPercent = 0.1F;

        private List<bool> setBorders;
        private List<CanvasControl> canvases;

        public ZoomButtonsControl()
        {
            this.InitializeComponent();

            setBorders = new List<bool>();
            canvases = new List<CanvasControl>();
        }

        private void Zoom(ZoomProperty widthProperty, ZoomProperty heightProperty)
        {
            if (Zoomed == null) return;

            Zoomed(this, new ZoomButtonsEventArgs(GetFactor(widthProperty, zoomFactorWidth),
                GetFactor(heightProperty, zoomFactorHeight)));
        }

        private void Canvases_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            CanvasControl canvas = sender as CanvasControl;

            AddCanvasToListAndResetOthers(canvas, true);
        }

        private void Canvases_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            CanvasControl canvas = sender as CanvasControl;

            AddCanvasToListAndResetOthers(canvas, false);
        }

        private void AddCanvasToListAndResetOthers(CanvasControl canvas, bool value)
        {
            if (!canvases.Contains(canvas))
            {
                canvases.Add(canvas);
                setBorders.Add(false);
            }

            for (int i = 0; i < canvases.Count; i++)
            {
                setBorders[i] = canvases[i] == canvas ? value : false;

                canvases[i].Invalidate();
            }
        }

        private void CanvasTM_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Zoom(ZoomProperty.Stay, ZoomProperty.In);
        }

        private void CanvasBM_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Zoom(ZoomProperty.Stay, ZoomProperty.Out);
        }

        private void CanvasMR_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Zoom(ZoomProperty.In, ZoomProperty.Stay);
        }

        private void CanvasML_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Zoom(ZoomProperty.Out, ZoomProperty.Stay);
        }

        private void CanvasTR_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Zoom(ZoomProperty.In, ZoomProperty.In);
        }

        private void CanvasBL_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Zoom(ZoomProperty.Out, ZoomProperty.Out);
        }

        private void CanvasPlus_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            DrawVerticalLine(sender, args);
            DrawHorizontalLine(sender, args);
            DrawBorder(sender, args);
        }

        private void CanvasMinus_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            DrawHorizontalLine(sender, args);
            DrawBorder(sender, args);
        }

        private void DrawHorizontalLine(CanvasControl sender, CanvasDrawEventArgs args)
        {
            float thickness = (float)(sender.ActualWidth + sender.ActualHeight) / 2 * lineThicknessPercent;
            Vector2 leftPoint, rightPoint;
            Color color = Color.FromArgb(255, 0, 0, 0);

            leftPoint = new Vector2((float)sender.ActualWidth / 4F, (float)sender.ActualHeight / 2F);
            rightPoint = new Vector2((float)sender.ActualWidth / 4F * 3, (float)sender.ActualHeight / 2F);

            args.DrawingSession.DrawLine(leftPoint, rightPoint, color, thickness);
        }

        private void DrawVerticalLine(CanvasControl sender, CanvasDrawEventArgs args)
        {
            float thickness = (float)(sender.ActualWidth + sender.ActualHeight) / 2 * lineThicknessPercent;
            Vector2 topPoint, bottomPoint;
            Color color = Color.FromArgb(255, 0, 0, 0);

            topPoint = new Vector2((float)sender.ActualWidth / 2F, (float)sender.ActualHeight / 4F);
            bottomPoint = new Vector2((float)sender.ActualWidth / 2F, (float)sender.ActualHeight / 4F * 3);

            args.DrawingSession.DrawLine(topPoint, bottomPoint, color, thickness);
        }

        private void Canvas_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private void Cavases_PointerMoved(object sender, PointerRoutedEventArgs e)
        {

        }

        private void DrawBorder(CanvasControl sender, CanvasDrawEventArgs args)
        {
            int index = canvases.IndexOf(sender);

            if (index < 0 || !setBorders[index]) return;

            float thickness = (float)(sender.ActualWidth + sender.ActualHeight) / 2F * borderAroundCanvasPercent;
            Color color = Color.FromArgb(255, 0, 0, 0);
            Rect rect = new Rect(0, 0, sender.ActualWidth, sender.ActualHeight);

            args.DrawingSession.DrawRectangle(rect, color, thickness);
        }

        private float GetFactor(ZoomProperty property, float factor)
        {
            switch (property)
            {
                case ZoomProperty.In:
                    return factor;

                case ZoomProperty.Out:
                    return 1 / factor;

                case ZoomProperty.Stay:
                    return 1;
            }

            return 1;
        }
    }
}
