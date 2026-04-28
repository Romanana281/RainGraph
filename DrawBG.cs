using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace Project2
{
    internal class DrawBG
    {
        public void BackwardDiagonal(Graphics g, Color color1, Color color2, Rectangle rect)
        {
            float step = 30f;

            Brush brush = new SolidBrush(color1);
            g.FillRectangle(brush, rect);

            Pen pen = new Pen(color2, 5f);
            for (float x = -rect.Height; x < rect.Width; x += step)
            {
                g.DrawLine(pen, x, 0, x + rect.Height, rect.Height);
            }

        }

        public void LargeCheckerBoard(Graphics g, Color color1, Color color2, Rectangle rect)
        {
            float step = 70f;

            for (float x = 0; x < rect.Width; x += step)
            {
                for (float y = 0; y < rect.Height; y += step)
                {
                    Brush brush = new SolidBrush(((x / step) + (y / step)) % 2 == 0 ? color1 : color2);
                    g.FillRectangle(brush, x, y, x + step, y + step);
                }
            }
        }

        public void ZigZag(Graphics g, Color color1, Color color2, Rectangle rect)
        {
            Brush brush = new SolidBrush(color1);
            g.FillRectangle(brush, rect);


            Pen pen = new Pen(color2, 5f);

            float step = 30;
            for (float y = 0; y < rect.Height; y += step)
            {
                var points = new PointF[rect.Width / 10 + 2];
                int pointIndex = 0;

                for (int x = 0; x <= rect.Width; x += 10)
                {
                    float zigzag = (float)Math.Sin(x * 0.1f) * 8f;
                    points[pointIndex++] = new PointF(x, y + zigzag);
                }

                if (pointIndex > 1)
                    g.DrawLines(pen, points.Take(pointIndex).ToArray());
            }
        }


        public void HorizontalBrick(Graphics g, Color color1, Color color2, Rectangle rect)
        {
            int brickWidth = 80;
            int brickHeight = 30;

            for (int y = 0; y < rect.Height; y += brickHeight)
            {
                for (int x = 0; x < rect.Width; x += brickWidth)
                {
                    bool isEvenBrick = (x / brickWidth + y / brickHeight) % 2 == 0;
                    Color brickColor = isEvenBrick ? color1 : color2;

                    using (var brush = new SolidBrush(brickColor))
                        g.FillRectangle(brush, x, y, brickWidth, brickHeight);
                }
            }
        }

        public void DiagonalGradient(Graphics g, Color color1, Color color2, Rectangle rect)
        {
            LinearGradientBrush linear = new LinearGradientBrush(rect, color1, color2, LinearGradientMode.ForwardDiagonal);
            g.FillRectangle(linear, rect);
        }
    }
}
