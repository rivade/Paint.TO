namespace DrawingProgram;

public abstract class DrawTool : ITool
{
    public static Color drawingColor = Color.Black;
    public static int brushRadius = 1;

    protected static readonly object lockObj = new();

    protected static Vector2 lastMousePos;
    public static void UpdateLastMousePos(Vector2 mousePos) => lastMousePos = mousePos;

    public virtual void Update(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            if (!PaletteButton.paletteColors.Contains(drawingColor))
            {
                PaletteButton.paletteColors.Enqueue(drawingColor);
                PaletteButton.LimitQueueSize();
            }
        }
    }

    // Bresenhams line algorithm
    public static void DrawThickLine(Image canvas, Vector2 startPos, Vector2 endPos, Color color, bool drawOnCanvas)
    {
        // Avgör var på linjen den itererar, börjar på startpos
        int x = (int)startPos.X;
        int y = (int)startPos.Y;

        // Beräkna förändringen i x och y
        int dx = Math.Abs((int)endPos.X - x);
        int dy = Math.Abs((int)endPos.Y - y);
        // Definiera stepsize för x och y
        int sx = x < (int)endPos.X ? 1 : -1;
        int sy = y < (int)endPos.Y ? 1 : -1;
        // Initiera felmarginal (för att kunna hantera tjockleken på linjen)
        int error = dx - dy;

        // Loopa genom alla punkter på linjen med hjälp av Bresenham's algoritm
        while (true)
        {
            // Rita en cirkel på den aktuella punkten
            if (drawOnCanvas)
                Raylib.ImageDrawCircleV(ref canvas, new Vector2(x, y), brushRadius, color);
            else
                Raylib.DrawCircleV(new Vector2(x, y), brushRadius, color);

            // Om slutet av linjen har nåtts bryts loopen
            if (x == (int)endPos.X && y == (int)endPos.Y)
                break;

            // Beräkna nästa punkt på linjen baserat på felmarginalen
            int doubleError = 2 * error;
            if (doubleError > -dy)
            {
                error -= dy;
                x += sx;
            }
            if (doubleError < dx)
            {
                error += dx;
                y += sy;
            }
        }
    }
}

public sealed class Pencil : DrawTool
{
    public override void Update(Image canvas, Vector2 mousePos)
    {
        base.Update(canvas, mousePos);
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            lock (lockObj)
                Raylib.ImageDrawLine(ref canvas,
                (int)lastMousePos.X, (int)lastMousePos.Y,
                (int)mousePos.X, (int)mousePos.Y,
                drawingColor);
        }
    }
}

public sealed class PaintBrush : DrawTool
{
    public override void Update(Image canvas, Vector2 mousePos)
    {
        base.Update(canvas, mousePos);
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            lock (lockObj)
                DrawThickLine(canvas, lastMousePos, mousePos, drawingColor, true);
        }
    }
}

public sealed class Eraser : DrawTool
{
    public override void Update(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            lock (lockObj)
                DrawThickLine(canvas, lastMousePos, mousePos, new Color(0, 0, 0, 0), true);
        }
    }
}

public sealed class Checker : DrawTool
{
    public static int checkerSize = 5;

    public override void Update(Image canvas, Vector2 mousePos)
    {
        base.Update(canvas, mousePos);
        lock (lockObj)
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
                SetCheckers(canvas, mousePos, false);
            else if (Raylib.IsMouseButtonDown(MouseButton.Right))
                SetCheckers(canvas, mousePos, true);
        }
    }

    private void SetCheckers(Image canvas, Vector2 mousePos, bool offsetByOneUnit)
    {
        int rows = (int)Math.Ceiling((double)Canvas.CanvasHeight + Canvas.CanvasOffset / checkerSize);
        int cols = (int)Math.Ceiling((double)Canvas.CanvasWidth + Canvas.CanvasOffset / checkerSize);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int xPos = col * checkerSize;
                int yPos = row * checkerSize;

                if (offsetByOneUnit) xPos += checkerSize;

                Vector2 squareCenter = new Vector2(xPos + checkerSize / 2, yPos + checkerSize / 2);

                float distanceToMouse = Vector2.Distance(mousePos, squareCenter);

                if (distanceToMouse <= brushRadius)
                {
                    if ((row + col) % 2 == 0)
                        Raylib.ImageDrawRectangle(ref canvas, xPos, yPos, checkerSize, checkerSize, drawingColor);
                }
            }
        }
    }
}

public sealed class Bucket : DrawTool
{
    private static readonly Vector2 CanvasArea = new(Canvas.CanvasWidth + Canvas.CanvasOffset, Canvas.CanvasHeight + Canvas.CanvasOffset);

    public override async void Update(Image canvas, Vector2 mousePos)
    {
        base.Update(canvas, mousePos);
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Color targetColor = Raylib.GetImageColor(canvas, (int)mousePos.X, (int)mousePos.Y);
            await FloodFill(canvas, mousePos, targetColor);
        }
    }

    private static async Task FloodFill(Image img, Vector2 pt, Color targetColor)
    {
        if (targetColor.Equals(drawingColor))
        {
            return;
        }

        await Task.Run(() =>
        {
            lock (lockObj)
            {
                Stack<Vector2> pixels = new();

                pixels.Push(pt);
                while (pixels.Count != 0)
                {
                    Vector2 temp = pixels.Pop();
                    int y1 = (int)temp.Y;
                    while (y1 >= Canvas.CanvasOffset && Raylib.GetImageColor(img, (int)temp.X, y1).Equals(targetColor))
                    {
                        y1--;
                    }
                    y1++;
                    bool spanLeft = false;
                    bool spanRight = false;
                    while (y1 < CanvasArea.Y && Raylib.GetImageColor(img, (int)temp.X, y1).Equals(targetColor))
                    {
                        Raylib.ImageDrawPixel(ref img, (int)temp.X, y1, drawingColor);

                        if (!spanLeft && temp.X > Canvas.CanvasOffset && Raylib.GetImageColor(img, (int)temp.X - 1, y1).Equals(targetColor))
                        {
                            pixels.Push(new Vector2(temp.X - 1, y1));
                            spanLeft = true;
                        }
                        else if (spanLeft && temp.X - 1 == Canvas.CanvasOffset && !Raylib.GetImageColor(img, (int)temp.X - 1, y1).Equals(targetColor))
                        {
                            spanLeft = false;
                        }
                        if (!spanRight && temp.X < CanvasArea.X - 1 && Raylib.GetImageColor(img, (int)temp.X + 1, y1).Equals(targetColor))
                        {
                            pixels.Push(new Vector2(temp.X + 1, y1));
                            spanRight = true;
                        }
                        else if (spanRight && temp.X < CanvasArea.X - 1 && !Raylib.GetImageColor(img, (int)temp.X + 1, y1).Equals(targetColor))
                        {
                            spanRight = false;
                        }
                        y1++;
                    }
                }
            }
        });
    }
}

public sealed class EyeDropper : DrawTool
{
    public override void Update(Image canvas, Vector2 mousePos)
    {
        Rectangle canvasRect = new(0, 0, new Vector2(canvas.Width, canvas.Height));
        if (Raylib.IsMouseButtonDown(MouseButton.Left) && Raylib.CheckCollisionPointRec(mousePos, canvasRect))
        {
            drawingColor = Raylib.GetImageColor(canvas, (int)mousePos.X, (int)mousePos.Y);
        }
        base.Update(canvas, mousePos);
    }
}