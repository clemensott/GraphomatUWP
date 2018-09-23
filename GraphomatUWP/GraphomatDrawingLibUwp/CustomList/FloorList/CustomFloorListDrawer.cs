using System;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class CustomFloorListDrawer : CustomListDrawer<FloorList>
    {
        public CustomFloorListDrawer(Graph graph, ViewArgs args) : base(graph, args)
        {
        }

        protected override FloorList CreateValuePointList()
        {
            return new FloorList(Graph);
        }

        protected override void Move(Vector2 deltaValue)
        {
            throw new NotImplementedException();
        }

        protected override void MoveScrollView()
        {
            throw new NotImplementedException();
        }
    }
}
