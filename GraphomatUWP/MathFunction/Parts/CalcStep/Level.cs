using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    class Level
    {
        public int Value { get; private set; }

        public int Id { get; private set; }

        public Level(int value, int id)
        {
            Value = value;
            Id = id;
        }
    }
}
