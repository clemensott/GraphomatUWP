namespace GraphomatDrawingLibUwp.CustomList
{
    class Bool
    {
        public bool Value { get; set; }

        public Bool(bool value)
        {
            Value = value;
        }

        public static implicit operator bool(Bool obj) =>obj.Value;
    }
}
