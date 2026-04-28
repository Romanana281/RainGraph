using System;

namespace Project2
{
    internal class WaterPool
    {
        private const int max = 1000;
        public float X { get; set; }
        public float Y { get; set; }
        public float count { get; set; }
        public bool isFull { get; set; } = false;

        public float Level()
        {
            return (float)Math.Round(100 * count / max);
        }
    }
}
