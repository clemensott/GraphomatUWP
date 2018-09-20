using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class CustomDictionaryDrawer : CustomListDrawer
    {
        public CustomDictionaryDrawer(Graph graph, ViewArgs args) : base(graph, args)
        {
        }

        protected override ICustomList CreateValuePointList()
        {
            return new CustomDictionary(Graph);
        }

        protected override void Move(Vector2 deltaValue)
        {
        }

        protected override void MoveScrollView()
        {
        }
    }
}
