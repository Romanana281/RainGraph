using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    internal class SinFunction : AbstFunction
    {
        public override double calc(double a) {
            return Math.Sin(a);
        }

        public override string name()
        {
            return "Sin(x)";
        }
    }
}
