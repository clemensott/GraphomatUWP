using System;
using System.Numerics;

namespace GraphomatDrawingLibUwp.CustomList
{
    class FloorListNode
    {
        public const int RefX = 1, RefRangeX = 1, DenominatorBase = 10;

        private Vector2 value;
        private FloorListNode[] subs;

        public FloorListNode this[int index] { get { return subs[index]; } }

        public int Numerator { get; private set; }

        public int DenominatorExpo { get; private set; }

        public Vector2 Value { get { return GetValue(); } }

        public FloorListNode Next { get; private set; }

        public FloorListNode(int numerator, int denominatorExpo, Vector2 value, FloorListNode next)
        {
            Numerator = numerator;
            DenominatorExpo = denominatorExpo;
            this.value = value;

            Next = next;
            subs = new FloorListNode[DenominatorBase];
        }

        protected virtual Vector2 GetValue()
        {
            return value;
        }

        public FloorListNode GetNew(int index, Vector2 value, FloorListNode next)
        {
            return new FloorListNode(Numerator * DenominatorBase + index, DenominatorExpo - 1, value, next);
        }

        //public static int GetExpo(float beginX, float endX)
        //{
        //    Math.Floor(Math.Log((endX - beginX) / RefRangeX, DenominatorBase));
        //}

        public static float GetX(int numerator, int denominatorExpo)
        {
            return (float)(RefX + Math.Pow(DenominatorBase, denominatorExpo) * RefRangeX * numerator);
        }
    }
}
