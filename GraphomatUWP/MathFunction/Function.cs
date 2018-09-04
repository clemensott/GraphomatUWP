using System.ComponentModel;
using System.Xml.Serialization;

namespace MathFunction
{
    public class Function : INotifyPropertyChanged
    {
        private bool isPossible;
        private string original, improve;
        private Calculator calculator;

        [XmlIgnore]
        public double this[double x] { get { return GetResult(x); } }

        [XmlIgnore]
        public bool IsPossible
        {
            get { return isPossible; }
            set
            {
                if (isPossible == value) return;

                isPossible = value;
                NotifyPropertyChanged("IsPossible");
            }
        }

        public string Equation
        {
            get { return original; }
            set
            {
                if (IsOriginalEquation(value)) return;

                original = value;
                SetImprovedEquation();
                NotifyPropertyChanged("Equation");
                NotifyPropertyChanged("OriginalAndImprovedEquations");
                NotifyPropertyChanged("ImprovedEquation");
            }
        }

        [XmlIgnore]
        public string ImprovedEquation { get { return IsPossible ? improve : "0 (Equationerror)"; } }

        [XmlIgnore]
        public string OriginalAndImprovedEquations { get { return Equation + " = " + ImprovedEquation; } }

        public Function(string equation)
        {
            original = "";
            Equation = equation;
        }

        private bool IsOriginalEquation(string equation)
        {
            return equation.Replace(" ", "").ToLower() == original.Replace(" ", "").ToLower();
        }

        private void SetImprovedEquation()
        {
            FunctionParts parts;

            try
            {
                parts = FunctionConverter.GetCalcParts(original);
                calculator = new Calculator(parts);
                improve = parts.Equation;

                IsPossible = true;
            }
            catch
            {
                IsPossible = false;
            }
        }

        public double GetResult(double x)
        {
            return IsPossible ? calculator.GetResult(x) : 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            if (null == PropertyChanged) return;

            try
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
            catch { }
        }

        public override string ToString()
        {
            return Equation;
        }
    }
}
