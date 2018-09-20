using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class FirstFloorListSubNode : FloorListNode
    {
        private FloorListNode parent;

        public FirstFloorListSubNode(int numerator, int denominatorExpo, FloorListNode parent, FloorListNode next) : 
            base(numerator, denominatorExpo, parent.Value, next)
        {
            this.parent = parent;
        }

        protected override Vector2 GetValue()
        {
            return parent.Value;
        }
    }
}
