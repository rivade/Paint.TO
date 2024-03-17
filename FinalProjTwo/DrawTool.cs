using System.Runtime.InteropServices;

namespace DrawingProgram;

public abstract class DrawTool
{
    protected Vector2 lastMousePos;

    public static Color drawingColor = Color.Black;

    public static int brushRadiusSelectorInt = 0;
    private static int[] radiuses = [1, 5, 10, 15, 20, 30, 40, 50, 100];
    public static int brushRadius
    {
        get
        {
            if (brushRadiusSelectorInt >= radiuses.Length)
                brushRadiusSelectorInt = 0;

            return radiuses[brushRadiusSelectorInt];
        }

        set { }
    }


    public virtual void Stroke(Image canvas, Vector2 mousePos)
    {
        lastMousePos = mousePos;
    }

    // vvvv Tack chatgpt, youtube, stackoverflow och gud för denna algoritm nedan vvvv
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

public class Pencil : DrawTool
{
    public override void Stroke(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            Raylib.ImageDrawLine(ref canvas,
                (int)lastMousePos.X, (int)lastMousePos.Y,
                (int)mousePos.X, (int)mousePos.Y,
                drawingColor);
        }

        base.Stroke(canvas, mousePos);
    }
}

public class PaintBrush : DrawTool
{
    public override void Stroke(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            DrawThickLine(canvas, lastMousePos, mousePos, drawingColor, true);
        }

        base.Stroke(canvas, mousePos);
    }
}

public class Eraser : DrawTool
{
    public override void Stroke(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            DrawThickLine(canvas, lastMousePos, mousePos, new Color(0, 0, 0, 0), true);
        }

        base.Stroke(canvas, mousePos);
    }
}

public class Checker : DrawTool
{
    public static int checkerSizeInt = 0;
    private static int[] checkerSizes = [5, 10, 15, 20];
    public static int checkerSize
    {
        get
        {
            if (checkerSizeInt >= checkerSizes.Length)
                checkerSizeInt = 0;

            return checkerSizes[checkerSizeInt];
        }
    }


    public override void Stroke(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            SetCheckers(canvas, mousePos);
    }

    private void SetCheckers(Image canvas, Vector2 mousePos)
    {
        int rows = (int)Math.Ceiling((double)Canvas.CanvasHeight / checkerSize);
        int cols = (int)Math.Ceiling((double)Canvas.CanvasWidth / checkerSize);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int xPos = col * checkerSize;
                int yPos = row * checkerSize;

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

public class Bucket : DrawTool
{
    public override void Stroke(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Color targetColor = Raylib.GetImageColor(canvas, (int)mousePos.X, (int)mousePos.Y);
            FloodFill(canvas, mousePos, targetColor);
        }
    }

    private void FloodFill(Image img, Vector2 pt, Color targetColor)
    {
        if (targetColor.Equals(drawingColor))
        {
            return;
        }

        Stack<Vector2> pixels = new();

        pixels.Push(pt);
        while (pixels.Count != 0)
        {
            Vector2 temp = pixels.Pop();
            int y1 = (int)temp.Y;
            while (y1 >= 0 && Raylib.GetImageColor(img, (int)temp.X, y1).Equals(targetColor))
            {
                y1--;
            }
            y1++;
            bool spanLeft = false;
            bool spanRight = false;
            while (y1 < img.Height && Raylib.GetImageColor(img, (int)temp.X, y1).Equals(targetColor))
            {
                Raylib.ImageDrawPixel(ref img, (int)temp.X, y1, drawingColor);

                if (!spanLeft && temp.X > 0 && Raylib.GetImageColor(img, (int)temp.X - 1, y1).Equals(targetColor))
                {
                    pixels.Push(new Vector2(temp.X - 1, y1));
                    spanLeft = true;
                }
                else if (spanLeft && temp.X - 1 == 0 && !Raylib.GetImageColor(img, (int)temp.X - 1, y1).Equals(targetColor))
                {
                    spanLeft = false;
                }
                if (!spanRight && temp.X < img.Width - 1 && Raylib.GetImageColor(img, (int)temp.X + 1, y1).Equals(targetColor))
                {
                    pixels.Push(new Vector2(temp.X + 1, y1));
                    spanRight = true;
                }
                else if (spanRight && temp.X < img.Width - 1 && !Raylib.GetImageColor(img, (int)temp.X + 1, y1).Equals(targetColor))
                {
                    spanRight = false;
                }
                y1++;
            }
        }
    }
}

public class EyeDropper : DrawTool
{
    public override void Stroke(Image canvas, Vector2 mousePos)
    {
        Rectangle canvasRect = new(0, 0, new Vector2(canvas.Width, canvas.Height));
        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && Raylib.CheckCollisionPointRec(mousePos, canvasRect))
        {
            drawingColor = Raylib.GetImageColor(canvas, (int)mousePos.X, (int)mousePos.Y);
        }
    }
}