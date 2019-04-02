using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class ValuePointNode
    {
        public ValuePointNode Previous { get; set; }

        public ValuePointNode Next { get; set; }

        public Vector2 Value { get; }

        public ValuePointNode(ValuePointNode next, Vector2 value)
        {
            Next = next;
            Value = value;
        }
    }
}
