using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    internal class CustomTwoLinkListDrawer : CustomListDrawer<ValuePointLinkedList>
    {
        public CustomTwoLinkListDrawer(Graph graph, ViewArgs args) : base(graph, args)
        {
            UpdateFirstNode();
        }

        protected override ValuePointLinkedList CreateValuePointList()
        {
            return new ValuePointLinkedList(Graph);
        }

        protected override void Move(Vector2 deltaValue)
        {
            UpdateFirstNode();
        }

        protected override void MoveScrollView()
        {
            UpdateFirstNode();
        }

        private void UpdateFirstNode()
        {
            float left = ViewArgs.ValueDimensions.Left;

            if (valuePointList?.ViewBeginNode == null) return;

            while (valuePointList.ViewBeginNode.Previous != null && valuePointList.ViewBeginNode.Value.X > left)
            {
                valuePointList.ViewBeginNode = valuePointList.ViewBeginNode.Previous;
            }

            while (valuePointList.ViewBeginNode.Next != null && valuePointList.ViewBeginNode.Next.Value.X < left)
            {
                valuePointList.ViewBeginNode = valuePointList.ViewBeginNode.Next;
            }
        }
    }
}
