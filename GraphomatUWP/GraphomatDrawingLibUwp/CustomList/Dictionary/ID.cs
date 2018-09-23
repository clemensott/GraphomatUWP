using System;

namespace GraphomatDrawingLibUwp.CustomList
{
    struct ID : IEquatable<ID>
    {
        private int offset, digits, digitsLength;

        public int Offset
        {
            get { return offset; }
            set
            {
                offset = value;
                Value = GetValue();
            }
        }

        public int Digits
        {
            get { return digits; }
            set
            {
                digits = value;
                Value = GetValue();
            }
        }

        public int DigitsLength
        {
            get { return digitsLength; }
            set { digitsLength = value; }
        }


        public float Value { get; private set; }

        public ID(int offset, int digits) : this()
        {
            Offset = offset;
            Digits = digits;

            Value = GetValue();
        }

        public ID(float value, float minPrecision)
        {
            offset = (int)Math.Floor(Math.Log10(Math.Abs(value)));

            int minPrecisionDigits = (int)Math.Log10(minPrecision) - 1;
            digits = (int)(Math.Pow(0.1, minPrecisionDigits) * value);
            digitsLength = (int)Math.Log10(Math.Abs(digits));

            Value = GetValue(offset, digits, digitsLength);
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

        private float GetValue()
        {
            return GetValue(Offset, Digits, DigitsLength);
        }

        public static float GetValue(int offset, int digits, int digitsLength)
        {
            return (float)(Math.Pow(10, offset - digitsLength) * digits);
        }

        public override bool Equals(object obj)
        {
            return obj is ID && Equals((ID)obj);
        }

        public bool Equals(ID other)
        {
            return Value == Value;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
