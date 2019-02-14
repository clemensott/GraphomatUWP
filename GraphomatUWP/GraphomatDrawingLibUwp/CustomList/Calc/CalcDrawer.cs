using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class CalcDrawer : CustomListDrawer<CalcList>
    {
        public CalcDrawer(Graph graph, ViewArgs args) : base(graph, args)
        {
        }

        protected override CalcList CreateValuePointList()
        {
            return new CalcList(Graph);
        }

        protected override void Move(Vector2 deltaValue)
        {
        }

        protected override void MoveScrollView()
        {
        }
    }
}
