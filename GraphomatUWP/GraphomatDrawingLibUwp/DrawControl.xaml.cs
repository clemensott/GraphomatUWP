using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace GraphomatDrawingLibUwp
{
    public sealed partial class DrawControl : UserControl
    {
        public static readonly DependencyProperty OrdnerProperty = DependencyProperty.Register("Ordner", 
            typeof(IList<Graph>), typeof(DrawControl), new PropertyMetadata(new ObservableCollection<Graph>(),
                new PropertyChangedCallback(OnFirstPropertyChanged)));

        private static void OnFirstPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var s = sender as DrawControl;

            if (s != null)
            {
                s.children = e.NewValue as ObservableCollection<Graph>;
            }
        }

        private ObservableCollection<Graph> children;

        public IList<Graph> Children { get { return children; } }

        public DrawControl()
        {
            this.InitializeComponent();

            children.CollectionChanged += Children_CollectionChanged;
        }

        private void Children_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            //Invalidate();
        }

    }
}
