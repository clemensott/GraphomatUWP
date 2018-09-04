using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace GraphomatDrawingLibUwp
{
    delegate void MoveScrollEventHandler(object sender, MoveScrollEventArgs e);

    class MoveScollManager
    {
        public event MoveScrollEventHandler MoveScrollView;
        public const float OverRender = 3;
        private const int waitMoveMoreMillis = 20, periodMoveMoreMillis = 1;
        private const long refDeltaMoveTicks = 10 * TimeSpan.TicksPerMillisecond;
        private const double moveMoreSlowDownFactor = 0.95, minDeltaMiddleOfView = 0.1, ignoreMinDeltaMiddleOfView = 0.0001;
        private const float minDistancesBetweenPointersPercent = 0.05F;

        private static MoveScollManager instance;

        public static MoveScollManager Current
        {
            get
            {
                if (instance == null) instance = new MoveScollManager();

                return instance;
            }
        }

        private bool startAverageDistanceWidthPossible, startAverageDistanceHeightPossible;
        private int moveMoreCounter;
        private long preMoveTicks, preDeltaMoveTicks;
        private double startAveragePointX, startAveragePointY, startAverageDistanceWidth,
            startAverageDistanceHeight, startDeltaMiddleOfViewX, startDeltaMiddleOfViewY,
            preDeltaMiddleOfViewX, preDeltaMiddleOfViewY;
        private List<PointerPoint> pointerPoints;
        private CanvasControl canvas;
        private ViewDimensions startViewDimensions;
        private DisplayInformation displayInfo;
        private Vector2 rawSizeFloat, actualSizeFloat;
        private Size rawSizeDouble, actualSizeDouble;
        private DispatcherTimer moveMoreTimer;

        public Vector2 RawPixelSize
        {
            get
            {
                SetRawSizes();

                return rawSizeFloat;
            }
        }

        public Vector2 ActualPixelSize
        {
            get
            {
                SetActualSizes();

                return actualSizeFloat;
            }
        }

        public ViewDimensions CurrentViewDimensions { get; private set; }

        private MoveScollManager()
        {
            pointerPoints = new List<PointerPoint>();
            displayInfo = DisplayInformation.GetForCurrentView();

            moveMoreTimer = new DispatcherTimer();
            moveMoreTimer.Interval = TimeSpan.FromMilliseconds(periodMoveMoreMillis);
            moveMoreTimer.Tick += new EventHandler<object>(MoveMore);
        }

        private void SetRawSizes()
        {
            double rawWidth = GetRawWidth(), rawHeight = GetRawHeight();

            if (rawSizeFloat == null)
            {
                rawSizeDouble = new Size(rawWidth, rawHeight);
                rawSizeFloat = new Vector2(Convert.ToSingle(rawWidth),
                    Convert.ToSingle(rawHeight));
            }

            if (rawWidth != rawSizeDouble.Width || rawHeight != rawSizeDouble.Height)
            {
                rawSizeDouble.Width = rawWidth;
                rawSizeDouble.Height = rawHeight;

                rawSizeFloat.X = Convert.ToSingle(rawWidth);
                rawSizeFloat.Y = Convert.ToSingle(rawHeight);
            }
        }

        private double GetRawWidth()
        {
            return canvas.ActualWidth * displayInfo.RawPixelsPerViewPixel;
        }

        private double GetRawHeight()
        {
            return canvas.ActualWidth * displayInfo.RawPixelsPerViewPixel;
        }

        private void SetActualSizes()
        {
            DispatcherTimer dis = new DispatcherTimer();
            double actualWidth = canvas.ActualWidth, actualHeight = canvas.ActualHeight;

            if (actualSizeFloat == null)
            {
                actualSizeDouble = new Size(actualWidth, actualHeight);
                actualSizeFloat = new Vector2(Convert.ToSingle(actualWidth),
                    Convert.ToSingle(actualHeight));
            }

            if (actualWidth != actualSizeDouble.Width || actualHeight != actualSizeDouble.Height)
            {
                actualSizeDouble.Width = actualWidth;
                actualSizeDouble.Height = actualHeight;

                actualSizeFloat.X = Convert.ToSingle(actualWidth);
                actualSizeFloat.Y = Convert.ToSingle(actualHeight);
            }
        }

        public void SetEvents(CanvasControl canvas, UIElement grid, float viewValueWidth,
            float viewValueHeight, float middleOfViewX, float middleOfViewY)
        {
            this.canvas = canvas;

            CurrentViewDimensions = new ViewDimensions(viewValueWidth,
                viewValueHeight, middleOfViewX, middleOfViewY);

            grid.PointerPressed += Canvas_PointerPressed;
            grid.PointerMoved += Canvas_PointerMoved;
            grid.PointerReleased += Canvas_PointerReleased;
            grid.PointerWheelChanged += Canvas_PointerWheelChanged;

            canvas.SizeChanged += Canvas_SizeChanged;
        }

        private void Canvas_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pointerPoints.Add(e.GetCurrentPoint(canvas));

            var isBool = canvas.CapturePointer(e.Pointer);
            var list = canvas.PointerCaptures.ToArray();

            startAverageDistanceWidthPossible = startAverageDistanceHeightPossible = true;
            GetAverageDistanceBetweenPointers(out startAverageDistanceWidth, out startAverageDistanceHeight);

            GetAverageMiddlePointOfPointers(out startAveragePointX, out startAveragePointY);
            preMoveTicks = DateTime.Now.Ticks;

            startViewDimensions = CurrentViewDimensions.Clone();
        }

        private void Canvas_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            int pointerPointIndex;
            double newMiddleOfViewX, newMiddleOfViewY, newViewWidth, newViewHeight;

            pointerPointIndex = pointerPoints.FindIndex(x => x.PointerId == e.Pointer.PointerId);

            if (pointerPointIndex == -1) return;

            pointerPoints[pointerPointIndex] = e.GetCurrentPoint(canvas);

            GetNewViewSize(out newViewWidth, out newViewHeight);
            GetNewMiddlePoint(out newMiddleOfViewX, out newMiddleOfViewY);

            StartMoveMoreTimer();

            SetCreateMoveScrollEventArgsAndTriggerEvent(newViewWidth,
                newViewHeight, newMiddleOfViewX, newMiddleOfViewY);
        }

        private async void StartMoveMoreTimer()
        {
            long currentTicks = DateTime.Now.Ticks;
            preDeltaMoveTicks = currentTicks - preMoveTicks;
            preMoveTicks = currentTicks;

            await Task.Delay(waitMoveMoreMillis);

            if (pointerPoints.Count > 0) return;

            startDeltaMiddleOfViewX = preDeltaMiddleOfViewX * refDeltaMoveTicks / preDeltaMoveTicks;
            startDeltaMiddleOfViewY = preDeltaMiddleOfViewY * refDeltaMoveTicks / preDeltaMoveTicks;

            moveMoreTimer.Start();
        }

        private void MoveMore(object obj1, object obj2)
        {
            int preDeltaAbsFactorX, preDeltaAbsFactorY;
            long currentTicks;
            double newMiddleOfViewX, newMiddleOfViewY, delta;

            if (pointerPoints.Count > 0)
            {
                moveMoreTimer.Stop();
                return;
            }

            preDeltaAbsFactorX = startDeltaMiddleOfViewX > 0 ? 1 : -1;
            preDeltaAbsFactorY = startDeltaMiddleOfViewY > 0 ? 1 : -1;

            delta = 0.5;

            startDeltaMiddleOfViewX -= preDeltaAbsFactorX * delta;
            startDeltaMiddleOfViewY -= preDeltaAbsFactorY * delta;

            if (preDeltaAbsFactorX * startDeltaMiddleOfViewX < minDeltaMiddleOfView &&
                preDeltaAbsFactorY * startDeltaMiddleOfViewY < minDeltaMiddleOfView)
            {
                preDeltaMiddleOfViewX = 0;
                preDeltaMiddleOfViewY = 0;
                moveMoreTimer.Stop();
                return;
            }

            newMiddleOfViewX = CurrentViewDimensions.MiddleOfViewValuePoint.X - preDeltaMiddleOfViewX;
            newMiddleOfViewY = CurrentViewDimensions.MiddleOfViewValuePoint.Y - preDeltaMiddleOfViewY;

            SetCreateMoveScrollEventArgsAndTriggerEvent(CurrentViewDimensions.ViewValueSize.X,
                CurrentViewDimensions.ViewValueSize.Y, newMiddleOfViewX, newMiddleOfViewY);
        }

        private void GetNewViewSize(out double newViewWidth, out double newViewHeight)
        {
            double newAverageDistanceWidth, newAverageDistanceHeight,
                averageDistanceWidthRatio, averageDistanceHeightRatio;

            GetAverageDistanceBetweenPointers(out newAverageDistanceWidth, out newAverageDistanceHeight);

            averageDistanceWidthRatio = startAverageDistanceWidth / newAverageDistanceWidth;
            averageDistanceHeightRatio = startAverageDistanceHeight / newAverageDistanceHeight;

            if (averageDistanceWidthRatio != 0 && !double.IsNaN(averageDistanceWidthRatio) &&
                !double.IsInfinity(averageDistanceWidthRatio))
            {
                newViewWidth = startViewDimensions.ViewValueSize.X * averageDistanceWidthRatio;
            }
            else newViewWidth = CurrentViewDimensions.ViewValueSize.X;

            if (averageDistanceHeightRatio > 0 && !double.IsNaN(averageDistanceHeightRatio) &&
                !double.IsInfinity(averageDistanceHeightRatio))
            {
                newViewHeight = startViewDimensions.ViewValueSize.Y * averageDistanceHeightRatio;
            }
            else newViewHeight = CurrentViewDimensions.ViewValueSize.Y;
        }

        private void GetAverageDistanceBetweenPointers(out double width, out double height)
        {
            Vector2 actualSize = ActualPixelSize;
            width = height = 0;

            for (int i = 0; i < pointerPoints.Count; i++)
            {
                for (int j = i + 1; j < pointerPoints.Count; j++)
                {
                    width += pointerPoints[i].Position.X - pointerPoints[j].Position.X;
                    height += pointerPoints[i].Position.Y - pointerPoints[j].Position.Y;
                }
            }

            if (!startAverageDistanceWidthPossible || Math.Abs(width) <
                actualSize.X * minDistancesBetweenPointersPercent)
            {
                width = double.NaN;
            }

            if (!startAverageDistanceHeightPossible || Math.Abs(height) <
                actualSize.Y * minDistancesBetweenPointersPercent)
            {
                height = double.NaN;
            }
        }

        private void GetNewMiddlePoint(out double newMiddleOfViewX, out double newMiddleOfViewY)
        {
            double newAveragePointX, newAveragePointY, startMiddlePointX,
                startMiddlePointY, actualPixelPerValueX, actualPixelPerValueY,
                preDeltaMiddleOfViewXCache, preDeltaMiddleOfViewYCache;

            GetAverageMiddlePointOfPointers(out newAveragePointX, out newAveragePointY);

            startMiddlePointX = startViewDimensions.MiddleOfViewValuePoint.X;
            startMiddlePointY = startViewDimensions.MiddleOfViewValuePoint.Y;

            actualPixelPerValueX = canvas.ActualWidth / CurrentViewDimensions.ViewValueSize.X;
            actualPixelPerValueY = canvas.ActualHeight / CurrentViewDimensions.ViewValueSize.Y;

            newMiddleOfViewX = startMiddlePointX - (newAveragePointX - startAveragePointX) / actualPixelPerValueX;
            newMiddleOfViewY = startMiddlePointY - (newAveragePointY - startAveragePointY) / actualPixelPerValueY;

            preDeltaMiddleOfViewXCache = CurrentViewDimensions.MiddleOfViewValuePoint.X - newMiddleOfViewX;
            preDeltaMiddleOfViewYCache = CurrentViewDimensions.MiddleOfViewValuePoint.Y - newMiddleOfViewY;

            if (Math.Abs(preDeltaMiddleOfViewXCache) > ignoreMinDeltaMiddleOfView)
            {
                preDeltaMiddleOfViewX = preDeltaMiddleOfViewXCache;
            }

            if (Math.Abs(preDeltaMiddleOfViewYCache) > ignoreMinDeltaMiddleOfView)
            {
                preDeltaMiddleOfViewY = preDeltaMiddleOfViewYCache;
            }
        }

        private void GetAverageMiddlePointOfPointers(out double x, out double y)
        {
            int count = 0;

            x = y = 0;

            foreach (PointerPoint pp in pointerPoints)
            {
                x += pp.Position.X;
                y += pp.Position.Y;

                count++;
            }

            x /= count;
            y /= count;
        }

        private void Canvas_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            int index = pointerPoints.FindIndex(x => x.PointerId == e.Pointer.PointerId);

            if (index != -1)
            {
                pointerPoints.RemoveAt(index);
                canvas.ReleasePointerCapture(e.Pointer);

                startAverageDistanceWidthPossible = startAverageDistanceHeightPossible = true;
                GetAverageDistanceBetweenPointers(out startAverageDistanceWidth, out startAverageDistanceHeight);

                GetAverageMiddlePointOfPointers(out startAveragePointX, out startAveragePointY);

                startViewDimensions = CurrentViewDimensions.Clone();
            }
        }

        private void Canvas_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {

        }

        private void Canvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double newViewWidth = CurrentViewDimensions.ViewValueSize.X /
                e.PreviousSize.Width * e.NewSize.Width;
            double newViewHeight = CurrentViewDimensions.ViewValueSize.Y /
                e.PreviousSize.Height * e.NewSize.Height;

            double newMiddleOfViewX = CurrentViewDimensions.MiddleOfViewValuePoint.X;
            double newMiddleOfViewY = CurrentViewDimensions.MiddleOfViewValuePoint.Y;

            SetCreateMoveScrollEventArgsAndTriggerEvent(newViewWidth,
                newViewHeight, newMiddleOfViewX, newMiddleOfViewY);
        }

        private void SetCreateMoveScrollEventArgsAndTriggerEvent(double newViewWidth,
            double newViewHeight, double newMiddleOfViewX, double newMiddleOfViewY)
        {
            CurrentViewDimensions = new ViewDimensions(newViewWidth,
                newViewHeight, newMiddleOfViewX, newMiddleOfViewY);
            MoveScrollEventArgs args = GetMoveScrollEventArgs(CurrentViewDimensions);

            if (!AreViewDimensionsPossible(CurrentViewDimensions) && MoveScrollView == null) return;

            MoveScrollView(this, args);
            canvas.Invalidate();
        }

        private MoveScrollEventArgs GetMoveScrollEventArgs(ViewDimensions newViewDimensions)
        {
            return new MoveScrollEventArgs(newViewDimensions.Clone());
        }

        private bool AreViewDimensionsPossible(ViewDimensions viewDimensions)
        {
            if (float.IsInfinity(viewDimensions.BottomRightValuePoint.X)) return false;
            if (float.IsInfinity(viewDimensions.BottomRightValuePoint.Y)) return false;
            if (float.IsInfinity(viewDimensions.MiddleOfViewValuePoint.X)) return false;
            if (float.IsInfinity(viewDimensions.MiddleOfViewValuePoint.Y)) return false;
            if (float.IsInfinity(viewDimensions.TopLeftValuePoint.X)) return false;
            if (float.IsInfinity(viewDimensions.TopLeftValuePoint.Y)) return false;
            if (float.IsInfinity(viewDimensions.ViewValueSize.X)) return false;
            if (float.IsInfinity(viewDimensions.ViewValueSize.Y)) return false;

            if (float.IsNaN(viewDimensions.BottomRightValuePoint.X)) return false;
            if (float.IsNaN(viewDimensions.BottomRightValuePoint.Y)) return false;
            if (float.IsNaN(viewDimensions.MiddleOfViewValuePoint.X)) return false;
            if (float.IsNaN(viewDimensions.MiddleOfViewValuePoint.Y)) return false;
            if (float.IsNaN(viewDimensions.TopLeftValuePoint.X)) return false;
            if (float.IsNaN(viewDimensions.TopLeftValuePoint.Y)) return false;
            if (float.IsNaN(viewDimensions.ViewValueSize.X)) return false;
            if (float.IsNaN(viewDimensions.ViewValueSize.Y)) return false;

            return true;
        }

        public void RefreshCanvas()
        {
            canvas.Invalidate();
        }
    }
}
