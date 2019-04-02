using GraphomatDrawingLibUwp;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace GraphomatUWP
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class GraphEditPage : Page
    {
        private Graph tmpGraph, originalGraph;

        public GraphEditPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is Graph) originalGraph = e.Parameter as Graph;
            else originalGraph = new Graph();

            tmpGraph = new Graph(originalGraph.Name, originalGraph.OriginalEquation, originalGraph.Color);

            DataContext = tmpGraph;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            originalGraph.Name = tmpGraph.Name;
            originalGraph.OriginalEquation = tmpGraph.OriginalEquation;
            originalGraph.Color = tmpGraph.Color;

            if (!ViewModel.Current.Graphs.Contains(originalGraph)) ViewModel.Current.Graphs.Add(originalGraph);

            Frame.GoBack();
        }

        private void cpColor_colorChanged(object sender, EventArgs e)
        {
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
