using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    abstract class AbstFunction : IFunction
    {
        public double calc(string atext)
        {
            double.TryParse(atext, out double a);
            return calc(a);
        }

        public abstract double calc(double a);
        public abstract string name();
    }
}
