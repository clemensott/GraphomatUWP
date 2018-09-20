using System;

namespace GraphomatDrawingLibUwp.CustomList
{
    struct ID : IEquatable<ID>
    {
        public int Offset { get; set; }

        public int Digits { get; set; }

        public ID(int offset, int digits) : this()
        {
            Offset = offset;
            Digits = digits;
        }

        public ID(float value, float minPrecision)
        {
            Offset = (int)Math.Floor(Math.Log10(value));

            int minPrecisionDigits = (int)Math.Log10(minPrecision);
            Digits = (int)(Math.Pow(0.1, minPrecisionDigits) * value);
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
            return (float)(Math.Pow(10, Offset) * Digits);
        }

        public override bool Equals(object obj)
        {
            return obj is ID && Equals((ID)obj);
        }

        public bool Equals(ID other)
        {
            return GetValue() == other.GetValue();
        }

        public override int GetHashCode()
        {
            return GetValue().GetHashCode();
        }
    }
}
