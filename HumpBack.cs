using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project2
{
    public class HumpBack
    {
        public float CenterX { get; set; }
        public float CenterY { get; set; }
        public float Amplitude { get; set; }
        public float Sigma { get; set; }

        public double GetValue(double x)
        {
            double exponent = -Math.Pow(x - CenterX, 2) / (2 * Sigma * Sigma);
            return CenterY + Amplitude * Math.Exp(exponent);
        }
    }
}
