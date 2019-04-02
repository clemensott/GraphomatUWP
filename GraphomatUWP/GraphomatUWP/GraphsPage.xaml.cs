using GraphomatDrawingLibUwp;
using MyToolkit.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace GraphomatUWP
{
    public sealed partial class GraphsPage : Page
    {
        public GraphsPage()
        {
            this.InitializeComponent();

            DataContext = ViewModel.Current;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Parameter);
            base.OnNavigatedTo(e);
        }

        private void FindIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Graph graph = (Graph)((SymbolIcon)sender).DataContext;

            ViewModel.Current.SelectedGraphIndex = ViewModel.Current.Graphs.IndexOf(graph);

            Frame.GoBack();
        }

        private void EditIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Graph graph = (Graph)((SymbolIcon)sender).DataContext;

            Frame.Navigate(typeof(GraphEditPage), graph);
        }

        private void DeleteIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Graph graph = (Graph)((SymbolIcon) sender).DataContext;

            ViewModel.Current.Graphs.Remove(graph);
        }

        private void AddGraph_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GraphEditPage));
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DataGrid) sender).SelectedItem == null) return;

            ((DataGrid) sender).SelectedItem = null;
        }

        private void AddRandom_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.Current.Graphs.Add(RandomGraph.Get());

            //for (int i = 0; i < 10; i++)
            //{
            //       graphs.Add(new RandomGraph());
            //}
        }
    }
}
