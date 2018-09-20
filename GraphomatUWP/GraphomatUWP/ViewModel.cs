using GraphomatDrawingLibUwp;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace GraphomatUWP
{
    class ViewModel : INotifyPropertyChanged
    {
        private static ViewModel instance;

        public static ViewModel Current
        {
            get
            {
                if (instance == null) instance = new ViewModel();

                return instance;
            }
        }

        public static void Load()
        {
            Task<Data> loadGraphsTask = Data.Load();

            loadGraphsTask.Wait();

            Current.valueSize = loadGraphsTask.Result.ValueSize;
            Current.middleOfView = loadGraphsTask.Result.MiddleOfView;
            Current.graphs = loadGraphsTask.Result.Graphs;
        }

        private int selectedGraphIndex;
        private Vector2 valueSize, middleOfView;
        private ObservableCollection<Graph> graphs;

        public int SelectedGraphIndex
        {
            get { return selectedGraphIndex; }
            set
            {
                System.Diagnostics.Debug.WriteLine("SelectedView: " + System.DateTime.Now.Millisecond);
                if (selectedGraphIndex == value) return;

                selectedGraphIndex = value;
                NotifyPropertyChanged("SelectedGraphIndex");
            }
        }

        public Visibility NoGraphsVisibility
        {
            get { return Graphs.Count > 0 ? Visibility.Collapsed : Visibility.Visible; }
        }

        public Vector2 ValueSize
        {
            get { return valueSize; }
            set
            {
                if (value == valueSize) return;

                valueSize = value;
                NotifyPropertyChanged("ValueSize");
            }
        }

        public Vector2 MiddleOfView
        {
            get { return middleOfView; }
            set
            {
                if (value == middleOfView) return;

                middleOfView = value;
                NotifyPropertyChanged("MiddleOfView");
            }
        }

        public ObservableCollection<Graph> Graphs
        {
            get { return graphs; }
            set
            {
                if (graphs == value) return;

                graphs = value;
                NotifyPropertyChanged("Graphs");
                NotifyPropertyChanged("NoGraphsVisibility");
                Data.Save();
            }
        }

        private ViewModel()
        {
            graphs = new ObservableCollection<Graph>();

            selectedGraphIndex = -1;
            valueSize = new Vector2(10, 10);
            middleOfView = new Vector2(0, 0);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName)
        {
            if (null == PropertyChanged) return;

            try
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            catch { }
        }
    }
}
