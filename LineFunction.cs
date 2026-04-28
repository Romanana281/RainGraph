using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    internal class LineFunction : AbstFunction
    {
        public override double calc(double a)
        {
            return 2 * a + 5;
        }

        public override string name()
        {
            return "2x+5";
        }
    }
}
