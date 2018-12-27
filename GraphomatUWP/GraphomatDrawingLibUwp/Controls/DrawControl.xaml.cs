using GraphomatDrawingLibUwp.CustomList;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Graphics.Display;
using Windows.UI;
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
        public const float PixelBufferFactor = 3;
        private const float defaultValueWidthAndHeight = 10F, defaultMiddelOfView = 0F,
            minDistancesBetweenPointersPercent = 0.05F, showAutoZoomFactor = 1.1F;
        private const double selectPointOnGraphMaxDistancePercent = 0.1;

        public static readonly DependencyProperty ChildrenProperty = DependencyProperty.Register("Children",
            typeof(ObservableCollection<Graph>), typeof(DrawControl), new PropertyMetadata(new ObservableCollection<Graph>(),
                new PropertyChangedCallback(OnChildrenPropertyChanged)));

        private static void OnChildrenPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = sender as DrawControl;
            var oldValue = (ObservableCollection<Graph>)e.OldValue;
            var newValue = (ObservableCollection<Graph>)e.NewValue;

            if (oldValue != null) oldValue.CollectionChanged -= s.OnChildrenChanged;
            s.children = newValue;
            if (newValue != null) newValue.CollectionChanged += s.OnChildrenChanged;

            s.SetGraphDrawingList();
        }

        public static readonly DependencyProperty ValueSizeProperty = DependencyProperty.Register("ValueSize",
            typeof(Vector2), typeof(DrawControl), new PropertyMetadata(new Vector2(defaultValueWidthAndHeight,
                defaultValueWidthAndHeight), new PropertyChangedCallback(OnValueSizePropertyChanged)));

        private static void OnValueSizePropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = sender as DrawControl;
            Vector2 oldValue = (Vector2)e.NewValue;
            Vector2 newValue = (Vector2)e.OldValue;

            if (!IsOverZero(newValue) || IsInfinityOrNaN(newValue)) s.ValueSize = oldValue;
        }


        public static readonly DependencyProperty MiddleOfViewProperty = DependencyProperty.Register("MiddleOfView",
            typeof(Vector2), typeof(DrawControl), new PropertyMetadata(new Vector2(defaultMiddelOfView,
                defaultMiddelOfView), new PropertyChangedCallback(OnMiddleOfViewPropertyChanged)));

        private static void OnMiddleOfViewPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = sender as DrawControl;
            Vector2 oldValue = (Vector2)e.NewValue;
            Vector2 newValue = (Vector2)e.NewValue;

            if (IsInfinityOrNaN(newValue)) s.MiddleOfView = oldValue;
        }

        public static readonly DependencyProperty SelectedGraphIndexProperty = DependencyProperty.
            Register("SelectedGraphIndex", typeof(int), typeof(DrawControl),
            new PropertyMetadata(-1, new PropertyChangedCallback(OnSelectedGraphIndexPropertyChanged)));

        private static void OnSelectedGraphIndexPropertyChanged(DependencyObject sender,
            DependencyPropertyChangedEventArgs e)
        {
            var s = sender as DrawControl;
            int newValue = (int)e.NewValue;

            //if (!IsValidSelectionIndex(newValue, s.Children.Count)) s.SelectedGraphIndex = newValue = -1;

            s.ZoomToChild(s.childrenDrawing?.ElementAtOrDefault(newValue));
        }

        private string debugText = "";

        private bool isDrew, isMoving, startAverageDistanceWidthHighEnough, startAverageDistanceHeightHighEnough;
        private int graphNearPointerIndex;
        private float oldMiddleOfViewY, oldViewHeight;
        private double startAverageDistanceWidth, startAverageDistanceHeight;
        private List<PointerPoint> pointerPoints;
        private ViewValueDimensions startValueDimensions;
        private AxesGraph axes;
        private ObservableCollection<Graph> children;
        private List<GraphDrawer> childrenDrawing;
        private DisplayInformation displayInfo;

        public event TappedCurveEventHandler TappedCurve;
        public event DoubleTappedCurveEventHandler DoubleTappedCurve;

        internal ViewValueDimensions CurrentValueDimensions
        {
            get { return new ViewValueDimensions(ValueSize, MiddleOfView); }
            private set
            {
                ValueSize = value.Size;
                MiddleOfView = value.Middle;
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

        internal ViewArgs ViewArgs { get { return new ViewArgs(CurrentValueDimensions, CurrentViewPixelSize); } }

        public int SelectedGraphIndex
        {
            get { return (int)GetValue(SelectedGraphIndexProperty); }
            set { SetValue(SelectedGraphIndexProperty, value); }
        }

        public Vector2 ValueSize
        {
            get { return (Vector2)GetValue(ValueSizeProperty); }
            set { SetValue(ValueSizeProperty, value); }
        }

        public Vector2 MiddleOfView
        {
            get { return (Vector2)GetValue(MiddleOfViewProperty); }
            set { SetValue(MiddleOfViewProperty, value); }
        }

        public ObservableCollection<Graph> Children
        {
            get { return (ObservableCollection<Graph>)GetValue(ChildrenProperty); }
            set { SetValue(ChildrenProperty, value); }
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
            Children = new ObservableCollection<Graph>();
            childrenDrawing = new List<GraphDrawer>();
        }

        private void OnChildrenChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SetGraphDrawingList();
        }

        private void SetGraphDrawingList()
        {
            if (!PixelSizeLoaded) return;

            List<GraphDrawer> newChildrenDrawing = new List<GraphDrawer>();

            foreach (Graph graph in children)
            {
                int index = childrenDrawing.FindIndex(x => x.Graph == graph);

                if (index == -1) newChildrenDrawing.Add(GetGraphDrawer(graph));
                else newChildrenDrawing.Add(childrenDrawing[index]);
            }

            if (!childrenDrawing.SequenceEqual(newChildrenDrawing))
            {
                childrenDrawing = newChildrenDrawing;

                ZoomToChild(childrenDrawing.ElementAtOrDefault(SelectedGraphIndex));
            }
        }

        private GraphDrawer GetGraphDrawer(Graph graph)
        {
            return new CustomDictionaryDrawer(graph, ViewArgs);
        }

        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            displayInfo = DisplayInformation.GetForCurrentView();
            SetGraphDrawingList();

            ccDraw.Invalidate();
        }

        private void Control_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SetGraphNearPointerIndex(e.GetCurrentPoint(ccDraw).Position.ToVector2());
        }

        private void Control_PointerExited(object sender, PointerRoutedEventArgs e)
        {
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

            startValueDimensions = CurrentValueDimensions;
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
                newViewWidth = startValueDimensions.Width * averageDistanceWidthRatio;
            }
            else newViewWidth = CurrentValueDimensions.Width;

            if (averageDistanceHeightRatio > 0 && !double.IsNaN(averageDistanceHeightRatio) &&
                !double.IsInfinity(averageDistanceHeightRatio))
            {
                newViewHeight = startValueDimensions.Height * averageDistanceHeightRatio;
            }
            else newViewHeight = CurrentValueDimensions.Height;
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

            startValueDimensions = CurrentValueDimensions;
        }

        private void Control_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {

        }

        private void ZoomButtons_Zoomed(object sender, ZoomButtonsEventArgs args)
        {
            CurrentValueDimensions = args.GetChangedValueDimensions(CurrentValueDimensions);

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

            ccDraw.Invalidate();
        }

        private static bool IsInfinityOrNaN(Vector2 vector)
        {
            if (float.IsInfinity(vector.X)) return true;
            if (float.IsInfinity(vector.Y)) return true;

            if (float.IsNaN(vector.X)) return true;
            if (float.IsNaN(vector.Y)) return true;

            return false;
        }

        private static bool IsOverZero(Vector2 vector)
        {
            if (vector.X <= 0) return false;
            if (vector.Y <= 0) return false;

            return true;
        }

        private static bool IsValidSelectionIndex(int index, int count)
        {
            return index >= -1 && index < count;
        }

        private void Control_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            isMoving = true;
        }

        private void Control_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            isMoving = false;

            ccDraw.Invalidate();
        }

        private bool AreValueDimensionsPossible(ViewValueDimensions valueDimensions)
        {
            if (float.IsInfinity(valueDimensions.Middle.X)) return false;
            if (float.IsInfinity(valueDimensions.Middle.Y)) return false;
            if (float.IsInfinity(valueDimensions.Width)) return false;
            if (float.IsInfinity(valueDimensions.Height)) return false;

            if (float.IsNaN(valueDimensions.Middle.X)) return false;
            if (float.IsNaN(valueDimensions.Middle.Y)) return false;
            if (float.IsNaN(valueDimensions.Width)) return false;
            if (float.IsNaN(valueDimensions.Height)) return false;

            if (valueDimensions.Width <= 0) return false;
            if (valueDimensions.Height <= 0) return false;

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
                System.Diagnostics.Debug.WriteLine("SetToOld: {0}\t{1}", oldViewHeight, CurrentValueDimensions.Height);
                oldMiddleOfViewY = CurrentValueDimensions.Middle.Y;
                oldViewHeight = CurrentValueDimensions.Height;

                child.GetMinAndMaxValue(out minValue, out maxValue);

                MiddleOfView = new Vector2(MiddleOfView.X, (maxValue + minValue) / -2F);
                ValueSize = new Vector2(ValueSize.X, (maxValue - minValue) * showAutoZoomFactor);
            }

            SetViewDimensionToAxesAndChildren();
        }

        private void ccDraw_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            GraphDrawer tappedChild;

            if (SelectedGraphIndex != -1) RestoreOldValueDimensions();

            if (!GetNearestChildDrawingInRange(e.GetPosition(ccDraw).ToVector2(), out tappedChild)) return;
            if (DoubleTappedCurve == null) return;

            e.Handled = true;
            DoubleTappedCurve(this, tappedChild.Graph);
        }

        private void RestoreOldValueDimensions()
        {
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
                float distance;

                if (child.IsNearCurve(vector, out distance) && distance < min)
                {
                    min = distance;
                    nearestChildInRange = child;
                }
            }

            return nearestChildInRange != null;
        }

        private void CanvasControl_Draw(CanvasControl sender, CanvasDrawEventArgs args)
        {
            //System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
            int selectedIndex = SelectedGraphIndex;
            Vector2 actualPixelSize = CurrentViewPixelSize.ActualPixelSize;
            ViewArgs viewArgs = ViewArgs;

            Axes.Draw(args.DrawingSession, actualPixelSize);

            if (isMoving)
            {
                Parallel.For(0, childrenDrawing.Count, (i) =>
                {
                    var (geo, color, thickness) = drawGeo(i);
                    args.DrawingSession.DrawGeometry(geo, color, thickness);
                });
            }
            else
            {
                for (int i = 0; i < childrenDrawing.Count; i++)
                {
                    var (geo, color, thickness) = drawGeo(i);
                    args.DrawingSession.DrawGeometry(geo, color, thickness);
                }
            }

            //Debug((sw.ElapsedTicks + " ticks").PadLeft(18));
            lock (this)
            {
                isDrew = true;
            }

            (CanvasGeometry geo, Color color, float thickness) drawGeo(int i)
            {
                GraphDrawer childDrawing = childrenDrawing[i];
                bool isThick = i == selectedIndex || (!isMoving && i == graphNearPointerIndex);
                float thickness = GraphDrawer.Thickness * (isThick ? 2 : 1);

                childDrawing.ViewArgs = viewArgs;

                CanvasGeometry geometry = childDrawing.Draw(sender, isMoving);

                return (geometry, childDrawing.Graph.Color, thickness);
            }
        }

        private void Debug(object obj)
        {
            tblDebug.Text = debugText = string.Format("{0}\r\n{1}", obj, debugText);

            int maxLength = 20 * 10;
            if (debugText.Length > maxLength) debugText = debugText.Remove(maxLength);
        }
    }
}
