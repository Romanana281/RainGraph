using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Windows.Forms;
using static Project2.Main;
using static System.Net.Mime.MediaTypeNames;


namespace Project2
{
    public partial class Main : Form
    {
        private string graphicName;
        private float currentZoom = 1.0f;

        //Move
        private Point lastMousePos;
        private bool isDragging = false;
        private PointF graphOffset = PointF.Empty;

        // Default
        private Color color1 = Color.CornflowerBlue;
        private Color color2;
        private Color colorGraphic = Color.Black;
        private int ind;
        private bool isRain = false;

        //Random
        private Random random = new Random();
        private AbstFunction currentFunction;
        private List<AbstFunction> functions = new List<AbstFunction>()
        {
            new SinFunction(),
            new SquareFunction(),
            new TgFunction(),
            new CubeFunction(),
            new LineFunction()
        };

        private DrawBG draw = new DrawBG();
        private Point currentMousePos = Point.Empty;

        List<DrawRain> rainPoints = new List<DrawRain>();
        List<PointF> minPoint = new List<PointF>();
        List<WaterPool> water = new List<WaterPool>();
        List<HumpBack> humps = new List<HumpBack>();

        Timer timer = new Timer();

        public Main()
        {
            InitializeComponent();

            Text += $": {functions[0].name()}";
            graphicName = Text;
            currentFunction = functions[0];

            canvasPanel.MouseDown += CanvasPanel_MouseDown;
            canvasPanel.MouseDown += Click_Mouse;
            canvasPanel.MouseMove += CanvasPanel_MouseMove;
            canvasPanel.MouseUp += CanvasPanel_MouseUp;

            canvasPanel.Paint += CanvasPanel_Paint;
            canvasPanel.Resize += CanvasPanel_Resize;
            canvasPanel.MouseWheel += Mouse_Wheel;

            timer.Tick += SettingsRain;
            timer.Interval = 10;
        }

        private void CanvasPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                lastMousePos = e.Location;
                canvasPanel.Cursor = Cursors.SizeAll;
                canvasPanel.Invalidate();

            }
        }

        private void CanvasPanel_MouseMove(object sender, MouseEventArgs e)
        {
            currentMousePos = e.Location;

            if (isDragging)
            {
                float deltaX = (e.X - lastMousePos.X) / (50f * currentZoom);
                float deltaY = (e.Y - lastMousePos.Y) / (50f * currentZoom);

                graphOffset.X -= deltaX;
                graphOffset.Y += deltaY;

                lastMousePos = e.Location;
                UpdateRainTarget();

                canvasPanel.Invalidate();
            }
            else
            {
                canvasPanel.Invalidate();
            }
        }

        private void CanvasPanel_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                canvasPanel.Cursor = Cursors.Default;
            }
        }

        private void Click_Mouse(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                PointF mathPos = GetCoordinateFromScreen(e.X, e.Y);

                AddGaussianBump(mathPos.X, mathPos.Y);

                canvasPanel.Invalidate();
            }
        }

        private void AddGaussianBump(float centerX, float centerY)
        {

            float amplitude = 0.5f;
            float sigma = 0.3f;

            var existingBump = humps.Find(b =>
                Math.Abs(b.CenterX - centerX) < sigma * 0.5f);

            if (existingBump != null)
            {
                existingBump.Amplitude += 0.2f;
                existingBump.Sigma *= 1.1f;
            }
            else
            {
                humps.Add(new HumpBack
                {
                    CenterX = centerX,
                    CenterY = centerY,
                    Amplitude = amplitude,
                    Sigma = sigma
                });
            }
            UpdateRainTarget();

        }

        private PointF GetCoordinateFromScreen(int screenX, int screenY)
        {
            float mathX = (screenX - canvasPanel.Width / 2) / (50f * currentZoom) + graphOffset.X;
            float mathY = (canvasPanel.Height / 2 - screenY) / (50f * currentZoom) + graphOffset.Y;

            return new PointF(mathX, mathY);
        }

        private PointF GetScreenFromCoordinate(float mathX, float mathY)
        {
            float screenX = canvasPanel.Width / 2 + (mathX - graphOffset.X) * currentZoom * 50f;
            float screenY = canvasPanel.Height / 2 - (mathY - graphOffset.Y) * currentZoom * 50f;

            return new PointF(screenX, screenY);
        }

        private void CanvasPanel_Paint(object sender, PaintEventArgs e)
        {
            DrawToBuffer(e.Graphics);
        }

        private void CanvasPanel_Resize(object sender, EventArgs e)
        {
            UpdateRainTarget();

            canvasPanel.Invalidate();
        }

        private void Mouse_Wheel(object sender, MouseEventArgs e)
        {
            float oldZoom = currentZoom;
            if (e.Delta > 0)
            {
                currentZoom *= 1.1f;
            }
            else
            {
                currentZoom *= 0.9f;
            }

            currentZoom = Math.Max(0.5f, Math.Min(5f, currentZoom));


            if (isRain && Math.Abs(oldZoom - currentZoom) > 0.01f)
            {
                AdaptRainToZoom(oldZoom, currentZoom);
            }

            UpdateRainTarget();

            canvasPanel.Invalidate();
        }

        private void AdaptRainToZoom(float oldZoom, float newZoom)
        {
            foreach (DrawRain rain in rainPoints)
            {

                PointF mathPos = GetCoordinateFromScreen((int)rain.X, (int)rain.Y);

                PointF newScreenPos = GetScreenFromCoordinate(mathPos.X, mathPos.Y);
                rain.X = newScreenPos.X;
                rain.Y = newScreenPos.Y;

                rain.speed = random.Next(5, 10);
            }
        }

        private void DrawToBuffer(Graphics e)
        {
            Graphics g = e;
            if (color2.IsEmpty) g.Clear(color1);
            else DrawBackground(g, ind);

            DrawFunction(g);

            DrawCoordinate(g);

            if (!isDragging && currentMousePos != Point.Empty)
            {
                DrawYellowRectangle(g, currentMousePos);
            }

            if (Math.Abs(currentZoom - 1.0f) > 0.01f) DrawZoom(g);
            DrawCircle(g);
            DrawRain(g);
            DrawPool(g);
        }

        private void DrawBackground(Graphics g, int ind)
        {
            Rectangle rect = new Rectangle(0, 0, canvasPanel.Width, canvasPanel.Height);
            switch (ind)
            {
                case 2:
                    draw.BackwardDiagonal(g, color1, color2, rect);
                    break;
                case 3:
                    draw.LargeCheckerBoard(g, color1, color2, rect);
                    break;
                case 4:
                    draw.ZigZag(g, color1, color2, rect);
                    break;
                case 5:
                    draw.HorizontalBrick(g, color1, color2, rect);
                    break;
                case 6:
                    draw.DiagonalGradient(g, color1, color2, rect);
                    break;
            }
        }

        private void DrawCoordinate(Graphics g)
        {
            Pen pen = new Pen(Color.Black, 1f / currentZoom);
            //draw x line
            g.DrawLine(pen, 0, canvasPanel.Height / 2, canvasPanel.Width, canvasPanel.Height / 2);
            //draw y line
            g.DrawLine(pen, canvasPanel.Width / 2, 0, canvasPanel.Width / 2, canvasPanel.Height);

            //draw x and y
            Font font = new Font("Times New Roman", Math.Max(8f, 12f / currentZoom));
            Brush brush = new SolidBrush(Color.Black);
            float margin = 7f / currentZoom;

            g.DrawString("X", font, brush, canvasPanel.Width - margin * 2 - 10, canvasPanel.Height / 2 + margin);
            g.DrawString("Y", font, brush, canvasPanel.Width / 2 + margin, margin);

            //draw div
            float step = 50 * currentZoom;
            if (step < 60) step = 60;
            if (step > 200) step = 200;

            for (int i = -10; i <= 10; i++)
            {
                if (i == 0) continue;
                float x = canvasPanel.Width / 2 + i * step;

                if (x > 10 && x < canvasPanel.Width - 10)
                {
                    g.DrawLine(pen, x, canvasPanel.Height / 2 - 5 / currentZoom, x, canvasPanel.Height / 2 + 5 / currentZoom);
                }
            }

            for (int i = -7; i <= 7; i++)
            {
                if (i == 0) continue;
                float y = canvasPanel.Height / 2 - i * step;
                if (y > 10 && y < canvasPanel.Height - 10)
                {
                    g.DrawLine(pen, canvasPanel.Width / 2 - 5 / currentZoom, y, canvasPanel.Width / 2 + 5 / currentZoom, y);
                }
            }
            pen.Dispose();
        }

        private void DrawZoom(Graphics g)
        {
            Font font = new Font("Times New Roman", 14f);
            Brush brush = new SolidBrush(Color.Black);

            g.DrawString($"Масштаб: {currentZoom:F2}x", font, brush, canvasPanel.Width - 150, 10);
            font.Dispose();
            brush.Dispose();
        }

        private void DrawCircle(Graphics g)
        {
            float step = 50f * currentZoom;
            if (step < 60) step = 60;
            if (step > 200) step = 200;

            float radius = step;
            float x = canvasPanel.Width / 2 - radius;
            float y = canvasPanel.Height / 2 - radius;
            float diameter = radius * 2;

            Pen pen = new Pen(Color.Black, 1f / currentZoom);
            pen.DashStyle = DashStyle.Custom;
            pen.DashPattern = new float[] { 10f, 7f };

            g.DrawEllipse(pen, x, y, diameter, diameter);
            pen.Dispose();
        }

        private void DrawFunction(Graphics g)
        {
            float step = 0.001f / currentZoom;
            float xMin = -canvasPanel.Width / (50f * currentZoom) * 1.1f + graphOffset.X;
            float xMax = canvasPanel.Width / (50f * currentZoom) * 1.1f + graphOffset.X;

            Pen graphPen = new Pen(colorGraphic, 1f / currentZoom);

            PointF? previousPoint = null;

            for (float x = xMin; x <= xMax; x += step)
            {
                try
                {
                    double originalY = currentFunction.calc(x);
                    foreach (var bump in humps)
                    {
                        originalY += bump.GetValue(x) - bump.CenterY;
                    }

                    if (!double.IsInfinity(originalY) && !double.IsNaN(originalY) && Math.Abs(originalY) <= 1000)
                    {
                        float drawY = (float)originalY;
                        bool shouldBreak = false;

                        foreach (var pool in water)
                        {
                            float poolWidth = 0.2f + (pool.Level() / 100f) * 1.0f;
                            float leftBound = pool.X - poolWidth / 2;
                            float rightBound = pool.X + poolWidth / 2;

                            if (x >= leftBound && x <= rightBound)
                            {
                                float waterLevel = pool.Y + 0.1f + (pool.Level() / 500f) * 0.9f;

                                if (pool.isFull)
                                {
                                    float gapCenter = pool.X;
                                    float gapLeft = gapCenter - 0.09f / 2;
                                    float gapRight = gapCenter + 0.09f / 2;

                                    if (x >= gapLeft && x <= gapRight && (float)originalY < waterLevel)
                                    {
                                        shouldBreak = true;
                                        break;
                                    }
                                }

                                if ((float)originalY < waterLevel)
                                {
                                    float stretch = 1.0f + (pool.Level() / 500f) * 2.0f;
                                    drawY = waterLevel + ((float)originalY - waterLevel) * stretch;
                                    break;
                                }
                            }
                        }

                        if (shouldBreak)
                        {
                            previousPoint = null;
                            continue;
                        }

                        PointF currentPoint = GetScreenFromCoordinate(x, drawY);

                        if (previousPoint.HasValue)
                        {
                            float deltaX = Math.Abs(currentPoint.X - previousPoint.Value.X);
                            float deltaY = Math.Abs(currentPoint.Y - previousPoint.Value.Y);

                            if (deltaX < 25f && deltaY < 100f)
                            {
                                g.DrawLine(graphPen, previousPoint.Value, currentPoint);
                                previousPoint = currentPoint;
                            }
                            else
                            {
                                previousPoint = currentPoint;
                            }
                        }
                        else
                        {
                            previousPoint = currentPoint;
                        }
                    }
                    else
                    {
                        previousPoint = null;
                    }
                }
                catch
                {
                    previousPoint = null;
                }
            }

            graphPen.Dispose();
        }

        private void DrawYellowRectangle(Graphics g, Point mousePos)
        {
            PointF mathMousePos = GetCoordinateFromScreen(mousePos.X, mousePos.Y);
            float mouseX = mathMousePos.X;

            double functionY = currentFunction.calc(mouseX);

            if (double.IsInfinity(functionY) || double.IsNaN(functionY) || Math.Abs(functionY) > 1000)
            {
                return;
            }

            PointF screenStart = new PointF(canvasPanel.Width / 2, canvasPanel.Height / 2);

            PointF screenEnd = GetScreenFromCoordinate(mouseX, (float)functionY);

            RectangleF rect = new RectangleF(
                Math.Min(screenStart.X, screenEnd.X),
                Math.Min(screenStart.Y, screenEnd.Y),
                Math.Abs(screenEnd.X - screenStart.X),
                Math.Abs(screenEnd.Y - screenStart.Y)
            );

            double distance = Math.Sqrt(mouseX * mouseX + functionY * functionY);
            double maxDistance = 10.0;

            double color = 100 * Math.Min(1.0, distance / maxDistance);


            Brush yellowBrush = new SolidBrush(Color.FromArgb((int)color, Color.Yellow));
            g.FillRectangle(yellowBrush, rect);


            Font font = new Font("Times New Roman", Math.Max(8f, 12f / currentZoom));
            Brush textBrush = new SolidBrush(Color.Black);
            string coords = $"({mouseX:F2}, {functionY:F2})";
            g.DrawString(coords, font, textBrush, screenEnd.X + 5, screenEnd.Y + 5);

        }

        private void UpdateRainTarget()
        {
            FindMin();

            foreach (DrawRain rain in rainPoints)
            {
                PointF mathPos = GetCoordinateFromScreen((int)rain.X, (int)rain.Y);
                float currentX = mathPos.X;
                double currentY = GetTotalHeight(currentX);

                if (minPoint.Count > 0)
                {
                    float bestTarget = minPoint[0].X;
                    float minDistance = Math.Abs(currentX - minPoint[0].X);
                    bool foundValidTarget = false;

                    foreach (var min in minPoint)
                    {
                        float minX = min.X;
                        float distance = Math.Abs(currentX - minX);

                        if (IsPathDownhillWithBumps(currentX, minX, currentY))
                        {
                            if (!foundValidTarget || distance < minDistance)
                            {
                                bestTarget = minX;
                                minDistance = distance;
                                foundValidTarget = true;
                            }
                        }
                    }

                    if (!foundValidTarget)
                    {
                        bestTarget = FindNearestLocalMinimum(currentX, currentY);
                    }

                    rain.Target = bestTarget;
                }
                else
                {
                    rain.Target = -canvasPanel.Width / (50f * currentZoom) * 1.1f + graphOffset.X;
                }
            }
        }

        private double GetTotalHeight(float x)
        {
            double height = currentFunction.calc(x);

            foreach (var bump in humps)
            {
                height += bump.GetValue(x) - bump.CenterY;
            }

            return height;
        }

        private bool IsPathDownhillWithBumps(float startX, float endX, double startHeight)
        {
            int steps = 15;
            float step = (endX - startX) / steps;

            double previousHeight = startHeight;

            for (int i = 1; i <= steps; i++)
            {
                float checkX = startX + step * i;
                double checkHeight = GetTotalHeight(checkX);

                if (double.IsInfinity(checkHeight) || double.IsNaN(checkHeight))
                    return false;

                if (checkHeight > previousHeight + 0.05)
                    return false;

                previousHeight = checkHeight;
            }

            return true;
        }

        private float FindNearestLocalMinimum(float currentX, double currentHeight)
        {
            foreach (var min in minPoint)
            {
                if (Math.Abs(currentX - min.X) < 0.1f)
                    return min.X;
            }

            double leftHeight = GetTotalHeight(currentX - 0.05f);
            double rightHeight = GetTotalHeight(currentX + 0.05f);

            if (leftHeight < currentHeight && leftHeight < rightHeight)
            {
                var leftMins = minPoint.Where(m => m.X < currentX).OrderBy(m => Math.Abs(currentX - m.X)).ToList();
                if (leftMins.Count > 0)
                    return leftMins[0].X;
            }
            else if (rightHeight < currentHeight)
            {
                var rightMins = minPoint.Where(m => m.X > currentX).OrderBy(m => Math.Abs(currentX - m.X)).ToList();
                if (rightMins.Count > 0)
                    return rightMins[0].X;
            }

            return minPoint.OrderBy(m => Math.Abs(currentX - m.X)).First().X;
        }

        private void SettingsRain(object sender, EventArgs e)
        {
            int maxCount = (int)(300 * currentZoom);
            rainPoints.RemoveAll(rain => rain.Y > canvasPanel.Height * 1.2f || rain.Y < -canvasPanel.Height * 0.2f || rain.X < -canvasPanel.Width * 0.2f || rain.X > canvasPanel.Width * 1.2f);

            if (rainPoints.Count < maxCount)
            {
                for (int i = 0; i < maxCount - rainPoints.Count; i++)
                {
                    DrawRain rain = new DrawRain();
                    rain.Y = 0;
                    rain.X = random.Next(1, canvasPanel.Width - 1);
                    rain.speed = random.Next(5, 10);
                    rainPoints.Add(rain);
                }
                UpdateRainTarget();
            }

            for (int i = 0; i < rainPoints.Count; i++)
            {
                rainPoints[i].Y += rainPoints[i].speed;
            }

            canvasPanel.Invalidate();
        }

        private void FindMin()
        {
            if (currentFunction is TgFunction) return;

            minPoint.Clear();
            float step = 0.01f / currentZoom;

            float xMin = -canvasPanel.Width / (50f * currentZoom) * 1.1f + graphOffset.X;
            float xMax = canvasPanel.Width / (50f * currentZoom) * 1.1f + graphOffset.X;

            if (xMax - xMin < 0.1f) return;

            for (float x = xMin; x < xMax; x += step)
            {
                try
                {
                    double prevY = currentFunction.calc(x - step);
                    double currentY = currentFunction.calc(x);
                    double nextY = currentFunction.calc(x + step);

                    foreach (var bump in humps)
                    {
                        prevY += bump.GetValue(x - step) - bump.CenterY;
                        currentY += bump.GetValue(x) - bump.CenterY;
                        nextY += bump.GetValue(x + step) - bump.CenterY;
                    }

                    if (double.IsInfinity(currentY) || double.IsNaN(currentY) || Math.Abs(currentY) > 50)
                        continue;

                    if (!double.IsInfinity(prevY) && !double.IsNaN(prevY) &&
                        !double.IsInfinity(nextY) && !double.IsNaN(nextY) &&
                        currentY < prevY && currentY < nextY)
                    {
                        minPoint.Add(new PointF(x, (float)currentY));
                    }
                }
                catch
                {
                    continue;
                }
            }
        }

        private void DrawRain(Graphics g)
        {
            if (rainPoints.Count == 0) return;
            Pen pen = new Pen(Color.Blue, 2.5f);

            for (int i = 0; i < rainPoints.Count; i++)
            {
                PointF mathPos = GetCoordinateFromScreen((int)rainPoints[i].X, (int)rainPoints[i].Y);

                if (currentFunction is TgFunction)
                {
                    double functionY = currentFunction.calc(mathPos.X);

                    if (double.IsInfinity(functionY) || double.IsNaN(functionY))
                    {
                        rainPoints.RemoveAt(i--);
                        continue;
                    }

                    if (mathPos.Y > functionY)
                    {
                        g.DrawLine(pen, rainPoints[i].X, rainPoints[i].Y, rainPoints[i].X, rainPoints[i].Y + 6f);
                        rainPoints[i].Y += rainPoints[i].speed;
                    }
                    else
                    {
                        rainPoints[i].X -= rainPoints[i].speed * 0.2f;
                        rainPoints[i].Y = GetScreenFromCoordinate(mathPos.X, (float)functionY).Y;
                        g.DrawLine(pen, rainPoints[i].X, rainPoints[i].Y, rainPoints[i].X, rainPoints[i].Y - 6f);
                    }
                    continue;
                }

                PointF pos = GetCoordinateFromScreen((int)rainPoints[i].X, (int)rainPoints[i].Y);

                if (rainPoints[i].lifeTime++ > 200)
                {
                    rainPoints[i].Y = 0;
                    rainPoints[i].X = random.Next(1, canvasPanel.Width - 1);
                    rainPoints[i].speed = random.Next(5, 10);
                    rainPoints[i].lifeTime = 0;
                    UpdateRainTarget();
                }

                double originalY = currentFunction.calc(pos.X);

                foreach (var bump in humps)
                {
                    originalY += bump.GetValue(pos.X) - bump.CenterY;
                }

                bool isOverFullPoolWithGap = false;
                foreach (var pool in water)
                {
                    float poolWidth = 0.2f + (pool.Level() / 100f) * 1.0f;
                    float leftBound = pool.X - poolWidth / 2;
                    float rightBound = pool.X + poolWidth / 2;

                    if (pos.X >= leftBound && pos.X <= rightBound && pool.isFull)
                    {
                        float waterLevel = pool.Y + 0.1f + (pool.Level() / 500f) * 0.9f;

                        float gapCenter = pool.X;
                        float gapLeft = gapCenter - 0.09f / 2;
                        float gapRight = gapCenter + 0.09f / 2;

                        if (pos.X >= gapLeft && pos.X <= gapRight && (float)originalY < waterLevel)
                        {
                            isOverFullPoolWithGap = true;
                            break;
                        }
                    }
                }

                double stretchedY = originalY;
                foreach (var pool in water)
                {
                    float poolWidth = 0.2f + (pool.Level() / 100f) * 1.0f;
                    float leftBound = pool.X - poolWidth / 2;
                    float rightBound = pool.X + poolWidth / 2;

                    if (pos.X >= leftBound && pos.X <= rightBound)
                    {
                        float waterLevel = pool.Y + 0.1f + (pool.Level() / 500f) * 0.9f;

                        if ((float)originalY < waterLevel)
                        {
                            float stretch = 1.0f + (pool.Level() / 500f) * 2.0f;
                            stretchedY = waterLevel + (originalY - waterLevel) * stretch;
                            break;
                        }
                    }
                }

                PointF stretchedScreenPos = GetScreenFromCoordinate(pos.X, (float)stretchedY);

                if (isOverFullPoolWithGap)
                {
                    float bottomTarget = canvasPanel.Height;

                    g.DrawLine(pen, rainPoints[i].X, rainPoints[i].Y, rainPoints[i].X, rainPoints[i].Y + 6f);
                    rainPoints[i].Y += rainPoints[i].speed * 1.5f;

                    if (rainPoints[i].Y >= bottomTarget - 10)
                    {
                        rainPoints.RemoveAt(i--);
                    }
                    continue;
                }

                if (rainPoints[i].Y < stretchedScreenPos.Y - 2)
                {
                    g.DrawLine(pen, rainPoints[i].X, rainPoints[i].Y, rainPoints[i].X, rainPoints[i].Y + 6f);
                    rainPoints[i].Y += rainPoints[i].speed;
                }
                else
                {
                    rainPoints[i].Y = Math.Min(rainPoints[i].Y, stretchedScreenPos.Y);

                    double targetOriginalY = currentFunction.calc(rainPoints[i].Target);
                    double targetStretchedY = targetOriginalY;

                    foreach (var pool in water)
                    {
                        float poolWidth = 0.2f + (pool.Level() / 100f) * 1.0f;
                        float leftBound = pool.X - poolWidth / 2;
                        float rightBound = pool.X + poolWidth / 2;

                        if (rainPoints[i].Target >= leftBound && rainPoints[i].Target <= rightBound)
                        {
                            float waterLevel = pool.Y + 0.1f + (pool.Level() / 500f) * 0.9f;

                            if (targetOriginalY < waterLevel)
                            {
                                float stretch = 1.0f + (pool.Level() / 500f) * 2.0f;
                                targetStretchedY = waterLevel + (targetOriginalY - waterLevel) * stretch;
                                break;
                            }
                        }
                    }

                    PointF stretchedScreenTarget = GetScreenFromCoordinate(rainPoints[i].Target, (float)targetStretchedY);

                    if (Math.Abs(rainPoints[i].X - stretchedScreenTarget.X) < 3f)
                    {
                        WaterPool pool = water.Find(p => Math.Abs(p.X - rainPoints[i].Target) < 0.1f);

                        if (pool == null)
                        {
                            pool = new WaterPool
                            {
                                X = rainPoints[i].Target,
                                Y = (float)targetStretchedY,
                                count = 0
                            };
                            water.Add(pool);
                        }

                        if (!pool.isFull)
                        {
                            float scaleFactor = 1f / currentZoom;

                            int baseIncrement = 1;
                            int increment = (int)(baseIncrement * scaleFactor);

                            pool.count += Math.Max(1, increment);
                        }

                        rainPoints.RemoveAt(i--);
                        continue;
                    }

                    if (rainPoints[i].X < stretchedScreenTarget.X)
                        rainPoints[i].X += rainPoints[i].speed * 0.15f;
                    else
                        rainPoints[i].X -= rainPoints[i].speed * 0.15f;

                    PointF currentStretchedPos = GetScreenFromCoordinate(
                        GetCoordinateFromScreen((int)rainPoints[i].X, (int)rainPoints[i].Y).X,
                        (float)stretchedY);
                    rainPoints[i].Y = currentStretchedPos.Y;

                    g.DrawLine(pen, rainPoints[i].X, rainPoints[i].Y, rainPoints[i].X, rainPoints[i].Y - 6f);
                }
            }

            pen.Dispose();
        }

        void DrawPool(Graphics g)
        {
            if (water.Count == 0) return;

            Pen pen = new Pen(Color.DodgerBlue, 3.5f);

            for (int i = 0; i < water.Count; i++)
            {
                if (water[i].Level() >= 195f) water[i].isFull = true;

                if (water[i].isFull)
                {
                    water[i].count = Math.Max(0, water[i].count - 10f);
                }

                float level = water[i].Level() / 100;

                float poolWidth = 0.2f + (water[i].Level() / 100f) * 1.0f;
                poolWidth *= currentZoom;

                float waterLevel = water[i].Y + 0.1f + (water[i].Level() / 500f) * 0.9f;
                float stretch = 1.0f + (water[i].Level() / 500f) * 2.0f;

                List<PointF[]> lines = new List<PointF[]>();

                for (float j = 0; j <= level; j += 0.001f)
                {
                    float lineWidth = (j / 2) * currentZoom;
                    float l = water[i].X - lineWidth;
                    float r = water[i].X + lineWidth;

                    double y1 = currentFunction.calc(l);
                    double y2 = currentFunction.calc(r);

                    foreach (var bump in humps)
                    {
                        y1 += bump.GetValue(l) - bump.CenterY;
                        y2 += bump.GetValue(r) - bump.CenterY;
                    }

                    double drawY1 = y1;
                    double drawY2 = y2;

                    if (y1 < waterLevel)
                        drawY1 = waterLevel + (y1 - waterLevel) * stretch;
                    if (y2 < waterLevel)
                        drawY2 = waterLevel + (y2 - waterLevel) * stretch;

                    PointF pos1 = GetScreenFromCoordinate(l, (float)drawY1);
                    PointF pos2 = GetScreenFromCoordinate(r, (float)drawY2);

                    lines.Add(new PointF[] { pos1, pos2 });
                }

                foreach (var line in lines)
                {
                    g.DrawLine(pen, line[0].X, line[0].Y, line[1].X, line[1].Y);
                }
            }

            pen.Dispose();
        }

        private void save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();

            saveFile.Filter = "Png files (*.png)|*.png|All files (*.*)|*.*";
            saveFile.FilterIndex = 1;
            saveFile.RestoreDirectory = true;
            saveFile.FileName = Path.GetFileName(graphicName);

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Bitmap bitmap = new Bitmap(canvasPanel.Width, canvasPanel.Height);

                    canvasPanel.DrawToBitmap(bitmap, new Rectangle(0, 0, canvasPanel.Width, canvasPanel.Height));
                    bitmap.Save(saveFile.FileName, ImageFormat.Png);
                    MessageBox.Show("График успешно сохранён!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    bitmap.Dispose();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            saveFile.Dispose();
        }

        private void styleBack_Click(object sender, EventArgs e)
        {
            BackgroundStyle backgroundStyle = new BackgroundStyle();

            if (backgroundStyle.ShowDialog() == DialogResult.OK)
            {
                color1 = Color.FromArgb(200, backgroundStyle.color1);
                color2 = Color.FromArgb(200, backgroundStyle.color2);

                ind = backgroundStyle.index;

                canvasPanel.Invalidate();
            }
        }

        private void styleGraf_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.AllowFullOpen = true;
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                MessageBox.Show("Цвет изменен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                colorGraphic = dialog.Color;
                canvasPanel.Invalidate();
            }
        }

        private void randFunction_Click(object sender, EventArgs e)
        {
            rainPoints.Clear();
            water.Clear();

            int ind = random.Next(functions.Count);
            currentFunction = functions[ind];
            currentZoom = 1.0f;

            color1 = Color.CornflowerBlue;
            color2 = Color.Empty;
            colorGraphic = Color.Black;
            Text = "Конструктор графиков";

            Text += $": {currentFunction.name()}";
            graphicName = Text;
            graphOffset = PointF.Empty;

            canvasPanel.Invalidate();
        }

        private void TurnRain_Click(object sender, EventArgs e)
        {
            if (!isRain)
            {
                isRain = true;
                TurnRain.BackColor = Color.Green;
                TurnRain.Text = "Дождь (вкл.)";
                timer.Start();
            }
            else
            {
                isRain = false;
                TurnRain.BackColor = Color.Crimson;
                TurnRain.Text = "Дождь (выкл.)";
                timer.Stop();
                rainPoints.Clear();
                water.Clear();
                canvasPanel.Invalidate();
            }
        }
    }
}
