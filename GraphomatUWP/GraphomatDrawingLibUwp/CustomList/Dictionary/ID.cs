using System;

namespace GraphomatDrawingLibUwp.CustomList
{
    struct ID : IEquatable<ID>
    {
        public int Factor { get; set; }

        public int Digits { get; set; }

        public ID(int factor, int digits)
        {
            this.Factor = factor;
            this.Digits = digits;
        }

        public ID(float value, float minPrecision)
        {
            Factor = (int)Math.Log10(minPrecision) - 1;
            Digits = (int)(value / Math.Pow(10, Factor));
        }

        public ID GetNext()
        {
            ID id = this;

            return id.SetNext();
        }

        public ID SetNext()
        {
            Digits++;

            return this;
        }

        public float GetValue()
        {
            return GetValue(Factor, Digits);
        }

        public static float GetValue(int factor, int digits)
        {
            return (float)(Math.Pow(10, factor) * digits);
        }

        public override bool Equals(object obj)
        {
            return obj is ID && Equals((ID)obj);
        }

        public bool Equals(ID other)
        {
            return Digits == other.Digits && Factor == other.Factor;
        }

        public override int GetHashCode()
        {
            var hashCode = 1429960089;
            hashCode = hashCode * -1521134295 + Factor.GetHashCode();
            hashCode = hashCode * -1521134295 + Digits.GetHashCode();
            return hashCode;
        }
    }
}
