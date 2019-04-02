using GraphomatDrawingLibUwp;
using Windows.Phone.UI.Input;
using Windows.System.Profile;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace GraphomatUWP
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            DataContext = ViewModel.Current;

            if ("Windows.Mobile" == AnalyticsInfo.VersionInfo.DeviceFamily)
            {
                HardwareButtons.BackPressed += HardwareButtons_BackPressed;
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            if (Frame.BackStackDepth == 0) return;

            Frame.GoBack();
            e.Handled = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //Graph graph1 = new Graph("Herz Links", "(-x)^(2/3)+0,9*(3,3-x^2)^(1/2)*sin(80* pi*x)", Windows.UI.Colors.Red);
            //Graph graph2 = new Graph("Herz Rechts", "x^(2/3)+0,9*(3,3-x^2)^(1/2)*sin(80* pi*x)", Windows.UI.Colors.Red);
            //Graph graph1 = new Graph("Log", "(lgx)^5", Windows.UI.Colors.Blue);
            //Graph graph2 = new Graph("N", "(lgx", Windows.UI.Colors.Red);

            //ViewModel.Current.Graphs.Clear();
            //ViewModel.Current.Graphs.Add(graph1);
            //ViewModel.Current.Graphs.Add(graph2);
        }

        private void EquationAppBar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GraphsPage), "Appbar");
        }

        private void DrawControl_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(GraphsPage), "Double");
        }

        private void DrawControl_DoubleTappedCurve(object sender, Graph tappedGraph)
        {
            Frame.Navigate(typeof(GraphEditPage), tappedGraph);
        }
    }
}
