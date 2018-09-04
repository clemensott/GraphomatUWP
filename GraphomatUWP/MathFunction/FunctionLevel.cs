using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MathFunction
{
    class FunctionLevel : IResult, INotifyPropertyChanged
    {
        private bool isPossible;
        private string original;

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

        public string OriginalEquation
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
        public string ImprovedEquation
        {
            get { return original; }
        }

        public FunctionLevel(string equation)
        {
            original = "";
            OriginalEquation = equation;
        }

        private bool IsOriginalEquation(string equation)
        {
            return equation.Replace(" ", "").ToLower() == original.Replace(" ", "").ToLower();
        }

        private void SetImprovedEquation()
        {
            try
            {

                IsPossible = true;
            }
            catch
            {
                IsPossible = false;
            }
        }

        public void SetValues(FunctionParts parts)
        {
            throw new NotImplementedException();
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
            return OriginalEquation;
        }
    }
}
