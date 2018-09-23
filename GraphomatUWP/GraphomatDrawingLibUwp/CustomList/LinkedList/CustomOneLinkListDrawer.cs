using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    internal class CustomOneLinkListDrawer : CustomListDrawer<ValuePointLinkedList>
    {
        public CustomOneLinkListDrawer(Graph graph, ViewArgs args) : base(graph, args)
        {
        }

        protected override ValuePointLinkedList CreateValuePointList()
        {
            return new ValuePointLinkedList(Graph);
        }

        protected override void Move(Vector2 deltaValue)
        {
        }

        protected override void MoveScrollView()
        {
        }
    }
}
