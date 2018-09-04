using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathFunction
{
    interface IResult
    {
        double this[double x] { get; }

        double GetResult(double x);
    }
}
