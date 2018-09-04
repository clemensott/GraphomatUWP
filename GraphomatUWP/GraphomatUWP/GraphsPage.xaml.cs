using GraphomatDrawingLibUwp;
using MyToolkit.Controls;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace GraphomatUWP
{
    public sealed partial class GraphsPage : Page
    {
        public GraphsPage()
        {
            this.InitializeComponent();

            DataContext = ViewModel.Current;
        }

        private void FindIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Graph graph = (sender as SymbolIcon).DataContext as Graph;

            ViewModel.Current.SelectedGraphIndex = ViewModel.Current.Graphs.IndexOf(graph);

            Frame.GoBack();
        }

        private void EditIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Graph graph = (sender as SymbolIcon).DataContext as Graph;

            Frame.Navigate(typeof(GraphEditPage), graph);
        }

        private void DeleteIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Graph graph = (sender as SymbolIcon).DataContext as Graph;

            List<Graph> newList = new List<Graph>(ViewModel.Current.Graphs);
            newList.Remove(graph);

            ViewModel.Current.Graphs = newList;
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
            if ((sender as DataGrid).SelectedItem == null) return;

            (sender as DataGrid).SelectedItem = null;
        }
    }
}
