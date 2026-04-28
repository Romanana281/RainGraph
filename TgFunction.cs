using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    internal class TgFunction : AbstFunction
    {
        public override double calc(double a)
        {
            return Math.Tan(a);
        }

        public override string name()
        {
            return "Tan(x)";
        }
    }
}
