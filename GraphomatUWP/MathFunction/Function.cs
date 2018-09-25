using System;
using System.ComponentModel;
using System.Linq;
using System.Xml.Serialization;

namespace MathFunction
{
    public class Function : INotifyPropertyChanged
    {
        private bool isPossible;
        private string original;
        private PartBracket lowestLevel;

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

        public double this[double x] { get { return GetResult(x); } }

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
        public string ImprovedEquation { get { return GetImprovedEquation(); } }

        [XmlIgnore]
        public string OriginalAndImprovedEquations { get { return OriginalEquation + " = " + ImprovedEquation; } }

        public Function(string equation)
        {
            original = "";
            OriginalEquation = equation;
        }

        private bool IsOriginalEquation(string equation)
        {
            return equation.Replace(" ", "").ToLower() == original.Replace(" ", "").ToLower();
        }

        private string GetImprovedEquation()
        {
            if (!IsPossible) return "0 (Error)";

            string improved = lowestLevel.ToEquationString();

            return improved.Remove(improved.Length - 1).Remove(0, 1);
        }

        private void SetImprovedEquation()
        {
            try
            {
                lowestLevel = new PartBracket(original);
                IsPossible = true;
            }
            catch
            {
                IsPossible = false;
            }
        }

        public double GetResult(double x)
        {
            return IsPossible ? lowestLevel.GetResult(x) : 0;
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

        public static string GetRandomEquation(Random ran)
        {
            Part[] allTypes = Parts.GetAllTypes().ToArray();

            int partsLenght = ran.Next(5, 20);
            string equation = "";

            for (int i = 0; i < partsLenght; i++)
            {
                int partType = ran.Next(allTypes.Length + 1);

                equation += partType == allTypes.Length ? GetRandomNumber(ran) : allTypes[partType].ToEquationString();
            }

            return equation;
        }

        private static string GetRandomNumber(System.Random ran)
        {
            return (ran.NextDouble() * ran.Next(-100000, 100000)).ToString();
        }
    }
}
