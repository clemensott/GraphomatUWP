using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace GraphomatDrawingLibUwp
{
    public delegate void TappedCurveEventHandler(object sender, Graph tappedGraph);
    public delegate void DoubleTappedCurveEventHandler(object sender, Graph tappedGraph);

    public sealed partial class DrawControl : UserControl
    {
        private long ticks;

        private void StartTimer()
        {
            ticks = DateTime.Now.Ticks;
        }

        private long GetTimerMillis()
        {
            long delta = DateTime.Now.Ticks - ticks;

            return delta / TimeSpan.TicksPerMillisecond;
        }

        public const float PixelBufferFactor = 3;
        private const float defaultValueWidthAndHeight = 10F, defaultMiddelOfView = 0F,
            minDistancesBetweenPointersPercent = 0.05F, showAutoZoomPercent = 1.1F;
        private const double selectPointOnGraphMaxDistancePercent = 0.1;

        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register("Children",
            typeof(List<Graph>), typeof(DrawControl), new PropertyMetadata(new List<Graph>(),
                new PropertyChangedCallback(OnChildrenPropertyChanged)));

        private static void OnChildrenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = sender as DrawControl;

            s.Children = e.NewValue as List<Graph>;
            s.children = e.NewValue as List<Graph>;
            s.SetGraphDrawingList();
        }

        public static readonly DependencyProperty ValueSizeProperty = DependencyProperty.Register("ValueSize",
            typeof(Vector2), typeof(DrawControl), new PropertyMetadata(new Vector2(defaultValueWidthAndHeight,
                defaultValueWidthAndHeight), new PropertyChangedCallback(OnValueSizePropertyChanged)));

        private static void OnValueSizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Vector2 value = (Vector2)e.NewValue;
            var s = sender as DrawControl;

            s.ValueSize = value;
        }


        public static readonly DependencyProperty MiddleOfViewProperty = DependencyProperty.Register("MiddleOfView",
            typeof(Vector2), typeof(DrawControl), new PropertyMetadata(new Vector2(defaultMiddelOfView,
                defaultMiddelOfView), new PropertyChangedCallback(OnMiddleOfViewPropertyChanged)));

        private static void OnMiddleOfViewPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Vector2 value = (Vector2)e.NewValue;
            var s = sender as DrawControl;

            s.MiddleOfView = value;
        }

        public static readonly DependencyProperty SelectedGraphIndexProperty = DependencyProperty.
            Register("SelectedGraphIndex", typeof(int), typeof(DrawControl),
            new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedGraphIndexPropertyChanged)));

        private static void OnSelectedGraphIndexPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            int value = (int)e.NewValue;
            var s = sender as DrawControl;

            s.SelectedGraphIndex = value;
        }

        private string debugText = "";

        private bool isDrew, startAverageDistanceWidthHighEnough, startAverageDistanceHeightHighEnough;
        private int graphNearPointerIndex;
        private float oldMiddleOfViewY, oldViewHeight;
        private double startAverageDistanceWidth, startAverageDistanceHeight;
        private List<PointerPoint> pointerPoints;
        private ViewDimensions startViewDimensions;
        private AxesGraph axes;
        private List<Graph> children;
        private List<GraphDrawer> childrenDrawing;
        private DisplayInformation displayInfo;

        public event TappedCurveEventHandler TappedCurve;
        public event DoubleTappedCurveEventHandler DoubleTappedCurve;

        internal ViewDimensions CurrentViewDimensions
        {
            get { return new ViewDimensions(ValueSize, MiddleOfView); }
            private set
            {
                ValueSize = value.ViewValueSize;
                MiddleOfView = value.MiddleOfViewValuePoint;
            }
        }

        internal ViewPixelSize CurrentViewPixelSize
        {
            get
            {
                return new ViewPixelSize(ActualWidth, ActualHeight,
                    displayInfo == null ? 1 : displayInfo.RawPixelsPerViewPixel);
            }
        }

        internal ViewArgs ViewArgs { get { return new ViewArgs(CurrentViewDimensions, CurrentViewPixelSize); } }

        public int SelectedGraphIndex
        {
            get { return (int)GetValue(SelectedGraphIndexProperty); }
            set
            {
                int selectedIndex = SelectedGraphIndex;

                if (value >= -1 && value < Children.Count && value != selectedIndex)
                {
                    selectedIndex = value;
                    SetValue(SelectedGraphIndexProperty, selectedIndex);
                }

                ZoomToChild(selectedIndex < 0 || childrenDrawing == null ||
                    selectedIndex >= childrenDrawing.Count ? null : childrenDrawing[selectedIndex]);
            }
        }

        public Vector2 ValueSize
        {
            get { return (Vector2)GetValue(ValueSizeProperty); }
            set
            {
                if (value == ValueSize || !IsOverZero(value) || IsInfinityOrNaN(value)) return;

                SetValue(ValueSizeProperty, value);
            }
        }

        public Vector2 MiddleOfView
        {
            get { return (Vector2)GetValue(MiddleOfViewProperty); }
            set
            {
                if (value == MiddleOfView || IsInfinityOrNaN(value)) return;

                SetValue(MiddleOfViewProperty, value);
            }
        }

        public List<Graph> Children
        {
            get { return (List<Graph>)GetValue(ChildrenProperty); }
            set
            {
                if (Children == value) return;

                SetValue(ChildrenProperty, value);
            }
        }

        private bool PixelSizeLoaded { get { return CurrentViewPixelSize.RawPixelWidth > 0; } }

        private AxesGraph Axes
        {
            get
            {
                if (axes == null) axes = new AxesGraph(ViewArgs);

                return axes;
            }
        }

        public DrawControl()
        {
            this.InitializeComponent();

            SelectedGraphIndex = -1;
            graphNearPointerIndex = -1;
            pointerPoints = new List<PointerPoint>();
            children = new List<Graph>();
            childrenDrawing = new List<GraphDrawer>();
        }

        private void SetGraphDrawingList()
        {
            if (!PixelSizeLoaded) return;

            List<GraphDrawer> newChildrenDrawing = new List<GraphDrawer>();

            foreach (Graph graph in children)
            {
                int index = childrenDrawing.FindIndex(x => x.Graph == graph);

                if (index == -1) newChildrenDrawing.Add(new GraphDrawer(graph, ViewArgs));
                else newChildrenDrawing.Add(childrenDrawing[index]);
            }

            childrenDrawing = newChildrenDrawing;
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            displayInfo = DisplayInformation.GetForCurrentView();
            SetGraphDrawingList();
        }

        private void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //if (enteredPointerCount == 0) foreach (GraphDrawing child in childrenDrawing) child.IsMoving = true;

            //enteredPointerCount++;

            SetGraphNearPointerIndex(e.GetCurrentPoint(ccDraw).Position.ToVector2());
        }

        private void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //enteredPointerCount--;

            //if (enteredPointerCount < 0) enteredPointerCount = 0;
            //if (enteredPointerCount == 0)
            //{
            //    foreach (GraphDrawing child in childrenDrawing) child.IsMoving = false;

            //    ccDraw.Invalidate();
            //}

            if (graphNearPointerIndex == -1) return;

            graphNearPointerIndex = -1;
            ccDraw.Invalidate();
        }

        private void Control_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            pointerPoints.Add(e.GetCurrentPoint(ccDraw));

            (sender as UIElement).CapturePointer(e.Pointer);

            startAverageDistanceWidthHighEnough = startAverageDistanceHeightHighEnough = true;
            GetAverageDistanceBetweenPointers(out startAverageDistanceWidth, out startAverageDistanceHeight);

            startViewDimensions = CurrentViewDimensions.Clone();
        }

        private void Control_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            int pointerPointIndex;
            double newViewWidth, newViewHeight;

            pointerPointIndex = pointerPoints.FindIndex(x => x.PointerId == e.Pointer.PointerId);

            if (pointerPointIndex == -1)
            {
                SetGraphNearPointerIndex(e.GetCurrentPoint(ccDraw).Position.ToVector2());
                return;
            }

            graphNearPointerIndex = -1;
            pointerPoints[pointerPointIndex] = e.GetCurrentPoint(ccDraw);

            if (pointerPoints.Count <= 1) return;

            lock (this)
            {
                if (!isDrew) return;

                isDrew = false;
            }

            GetNewViewSize(out newViewWidth, out newViewHeight);

            ValueSize = new Vector2((float)newViewWidth, (float)newViewHeight);
            SetViewDimensionToAxesAndChildren();
        }

        private void SetGraphNearPointerIndex(Vector2 pointerPosition)
        {
            int childIndex;
            GraphDrawer childInRange;

            GetNearestChildDrawingInRange(pointerPosition, out childInRange);

            childIndex = childrenDrawing.IndexOf(childInRange);

            if (childIndex == graphNearPointerIndex) return;

            graphNearPointerIndex = childIndex;
            ccDraw.Invalidate();
        }

        private void GetNewViewSize(out double newViewWidth, out double newViewHeight)
        {
            double newAverageDistanceWidth, newAverageDistanceHeight,
                averageDistanceWidthRatio, averageDistanceHeightRatio;

            GetAverageDistanceBetweenPointers(out newAverageDistanceWidth, out newAverageDistanceHeight);

            averageDistanceWidthRatio = startAverageDistanceWidth / newAverageDistanceWidth;
            averageDistanceHeightRatio = startAverageDistanceHeight / newAverageDistanceHeight;

            if (averageDistanceWidthRatio > 0 && !double.IsNaN(averageDistanceWidthRatio) &&
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
            Vector2 actualSize = CurrentViewPixelSize.ActualPixelSize;
            width = height = 0;

            for (int i = 0; i < pointerPoints.Count; i++)
            {
                for (int j = i + 1; j < pointerPoints.Count; j++)
                {
                    width += pointerPoints[i].Position.X - pointerPoints[j].Position.X;
                    height += pointerPoints[i].Position.Y - pointerPoints[j].Position.Y;
                }
            }

            if (!startAverageDistanceWidthHighEnough || Math.Abs(width) <
                actualSize.X * minDistancesBetweenPointersPercent)
            {
                width = double.NaN;
            }

            if (!startAverageDistanceHeightHighEnough || Math.Abs(height) <
                actualSize.Y * minDistancesBetweenPointersPercent)
            {
                height = double.NaN;
            }
        }

        private void Control_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            int index = pointerPoints.FindIndex(x => x.PointerId == e.Pointer.PointerId);

            if (index == -1) return;

            pointerPoints.RemoveAt(index);
            (sender as UIElement).ReleasePointerCapture(e.Pointer);

            startAverageDistanceWidthHighEnough = startAverageDistanceHeightHighEnough = true;
            GetAverageDistanceBetweenPointers(out startAverageDistanceWidth, out startAverageDistanceHeight);

            startViewDimensions = CurrentViewDimensions.Clone();
        }

        private void Control_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {

        }

        private void ZoomButtons_Zoomed(object sender, ZoomButtonsEventArgs args)
        {
            CurrentViewDimensions = args.GetChangedViewDimensions(CurrentViewDimensions);

            SetViewDimensionToAxesAndChildren();
        }

        private void Control_PointerCaptureLost(object sender, PointerRoutedEventArgs e)
        {
            int index = pointerPoints.FindIndex(x => x.PointerId == e.Pointer.PointerId);

            if (index == -1) return;

            pointerPoints.RemoveAt(index);
        }

        private void Control_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            MoveAxesAndChildren((float)e.Delta.Translation.X, (float)e.Delta.Translation.Y);
        }

        private void Control_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Axes.MoveScrollView(ViewArgs);
            ccDraw.Invalidate();
        }

        private void MoveAxesAndChildren(float deltaPixelX, float deltaPixelY)
        {
            Vector2 deltaPixel = new Vector2(deltaPixelX, deltaPixelY);
            Vector2 ActualSize = new Vector2((float)ActualWidth, (float)ActualHeight);
            Vector2 deltaValue = deltaPixel / ActualSize * ValueSize;

            MiddleOfView = MiddleOfView - deltaValue;

            SetViewDimensionToAxesAndChildren();
        }

        private void SetViewDimensionToAxesAndChildren()
        {
            ViewArgs args = ViewArgs;

            if (!PixelSizeLoaded) return;

            Axes.MoveScrollView(args);

            foreach (GraphDrawer child in childrenDrawing)
            {
                child.MoveScrollView(args);
            }

            ccDraw.Invalidate();
        }

        private bool IsInfinityOrNaN(Vector2 vector)
        {
            if (float.IsInfinity(vector.X)) return true;
            if (float.IsInfinity(vector.Y)) return true;

            if (float.IsNaN(vector.X)) return true;
            if (float.IsNaN(vector.Y)) return true;

            return false;
        }

        private bool IsOverZero(Vector2 vector)
        {
            if (vector.X <= 0) return false;
            if (vector.Y <= 0) return false;

            return true;
        }

        private void Control_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            foreach (GraphDrawer child in childrenDrawing) child.IsMoving = true;
        }

        private void Control_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            foreach (GraphDrawer child in childrenDrawing) child.IsMoving = false;

            ccDraw.Invalidate();
        }

        private bool AreViewDimensionsPossible(ViewDimensions viewDimensions)
        {
            if (float.IsInfinity(viewDimensions.MiddleOfViewValuePoint.X)) return false;
            if (float.IsInfinity(viewDimensions.MiddleOfViewValuePoint.Y)) return false;
            if (float.IsInfinity(viewDimensions.ViewValueSize.X)) return false;
            if (float.IsInfinity(viewDimensions.ViewValueSize.Y)) return false;

            if (float.IsNaN(viewDimensions.MiddleOfViewValuePoint.X)) return false;
            if (float.IsNaN(viewDimensions.MiddleOfViewValuePoint.Y)) return false;
            if (float.IsNaN(viewDimensions.ViewValueSize.X)) return false;
            if (float.IsNaN(viewDimensions.ViewValueSize.Y)) return false;

            if (viewDimensions.ViewValueSize.X <= 0) return false;
            if (viewDimensions.ViewValueSize.Y <= 0) return false;

            return true;
        }

        private void ccDraw_Tapped(object sender, TappedRoutedEventArgs e)
        {
            GraphDrawer tappedChild;

            bool findChild = GetNearestChildDrawingInRange(e.GetPosition(ccDraw).ToVector2(), out tappedChild);

            SelectedGraphIndex = childrenDrawing.IndexOf(tappedChild);
            ZoomToChild(tappedChild);

            if (!findChild) return;
            TappedCurve?.Invoke(this, tappedChild.Graph);

            e.Handled = true;
        }

        private void ZoomToChild(GraphDrawer child)
        {
            float minValue, maxValue;

            if (child != null)
            {
                System.Diagnostics.Debug.WriteLine("SetToOld: {0}\t{1}", oldViewHeight, CurrentViewDimensions.ViewValueSize.Y);
                oldMiddleOfViewY = CurrentViewDimensions.MiddleOfViewValuePoint.Y;
                oldViewHeight = CurrentViewDimensions.ViewValueSize.Y;

                child.GetMinAndMaxValue(out minValue, out maxValue);

                MiddleOfView = new Vector2(MiddleOfView.X, (maxValue + minValue) / -2F);
                ValueSize = new Vector2(ValueSize.X, (maxValue - minValue) * showAutoZoomPercent);
            }

            SetViewDimensionToAxesAndChildren();
        }

        private void ccDraw_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            GraphDrawer tappedChild;

            RestoreOldViewDimensions();

            if (!GetNearestChildDrawingInRange(e.GetPosition(ccDraw).ToVector2(), out tappedChild)) return;
            if (DoubleTappedCurve == null) return;

            e.Handled = true;
            DoubleTappedCurve(this, tappedChild.Graph);
        }

        private void RestoreOldViewDimensions()
        {
            System.Diagnostics.Debug.WriteLine("SetFromOld: {0}\t{1}", oldViewHeight, ValueSize.Y);
            MiddleOfView = new Vector2(MiddleOfView.X, oldMiddleOfViewY);
            ValueSize = new Vector2(ValueSize.X, oldViewHeight);

            SetViewDimensionToAxesAndChildren();
        }

        private bool GetNearestChildDrawingInRange(Vector2 vector, out GraphDrawer nearestChildInRange)
        {
            float min = float.MaxValue;
            nearestChildInRange = null;

            foreach (GraphDrawer child in childrenDrawing)
            {
                float distance = child.IsNearCurve(vector);

                if (min > distance)
                {
                    min = distance;
                    nearestChildInRange = child;
                }
            }

            return nearestChildInRange != null;
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            int selectedIndex = SelectedGraphIndex;
            Vector2 actualPixelSize = CurrentViewPixelSize.ActualPixelSize;
            CanvasGeometry[] gemotries = new CanvasGeometry[childrenDrawing.Count];
            Axes.Draw(args.DrawingSession, actualPixelSize);

            //DateTime now = DateTime.Now;
            for (int i = 0; i < childrenDrawing.Count; i++)
            {
                bool isThick = i == selectedIndex || i == graphNearPointerIndex;

                //  if (cbxWithBuilder.IsChecked == true)
                childrenDrawing[i].Draw2(sender, args.DrawingSession, actualPixelSize, isThick);
                //else childrenDrawing[i].Draw2(sender, args.DrawingSession, actualPixelSize, isThick);
            }

            //Debug((DateTime.Now - now).Ticks);
            lock (this)
            {
                isDrew = true;
            }
        }

        private void Debug(object obj)
        {
            tblDebug.Text = debugText = string.Format("{0}\n{1}", obj, debugText);

            if (debugText.Length > 100) debugText.Remove(100);
        }
    }
}
