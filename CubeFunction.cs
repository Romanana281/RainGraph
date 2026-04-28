using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    internal class CubeFunction : AbstFunction
    {
        public override double calc(double a)
        {
            return a * a * a;
        }

        public override string name()
        {
            return "x*x*x";
        }
    }
}
