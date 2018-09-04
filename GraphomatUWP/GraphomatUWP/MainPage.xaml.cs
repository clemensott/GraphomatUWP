using GraphomatDrawingLibUwp;
using System;
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
            Frame curFrame = Window.Current.Content as Frame;

            if (curFrame.BackStackDepth == 0) return;

            curFrame.GoBack();
            e.Handled = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void EquationAppBar_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GraphsPage));
        }

        private void DrawControl_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(GraphsPage)); 
        }

        private void DrawControl_DoubleTappedCurve(object sender, Graph tappedGraph)
        {
            Frame.Navigate(typeof(GraphEditPage), tappedGraph);
        }
    }
}
