using System.Numerics;

namespace GraphomatDrawingLibUwp.Values
{
    class ValuePointNode
    {
        public ValuePointNode Next { get; set; }

        public Vector2 Value { get; private set; }

        public ValuePointNode(ValuePointNode next, Vector2 value)
        {
            Next = next;
            Value = value;
        }
    }
}
