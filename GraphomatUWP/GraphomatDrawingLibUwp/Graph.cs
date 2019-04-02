using MathFunction;
using System.Xml.Serialization;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace GraphomatDrawingLibUwp
{
    public class Graph : Function
    {
        private string name;
        private Color color;
        private Brush brush;

        public Color Color
        {
            get => color;
            set
            {
                if (color == value) return;

                color = value;
                brush = new SolidColorBrush(color);

                NotifyPropertyChanged("Color");
                NotifyPropertyChanged("brush");
            }
        }

        [XmlIgnore]
        public Brush Brush => brush;

        public string Name
        {
            get => name;
            set
            {
                if (name == value) return;

                name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public Graph() : this("", "", Colors.Black)
        {

        }

        public Graph(string name, string equation, Color color) : base(equation)
        {
            Name = name;
            OriginalEquation = equation;
            Color = color;
        }
    }
}
