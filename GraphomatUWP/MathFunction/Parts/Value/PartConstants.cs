using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    class PartConstants : PartSimpleValue
    {
        private string[] names;

        public string Name { get { return ToEquationString(); } }

        public PartConstants(double value, params string[] names) : base(value)
        {
            this.names = names;
        }

        public PartConstants(string value, params string[] names) : base(value)
        {
            this.names = names;
        }

        protected override string[] GetLowerLooks()
        {
            return names;
        }
    }
}
