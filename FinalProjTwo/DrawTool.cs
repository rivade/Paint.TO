using System.Runtime.InteropServices;

namespace DrawingProgram;

public abstract class DrawTool
{
    protected Vector2 lastMousePos;
    public static Stack<Image> strokes = new();


    public static int colorInt = 0;
    private static Color[] colors = [Color.Black, Color.Red, Color.Orange, Color.Yellow, Color.Green, Color.DarkGreen, Color.Blue, Color.Violet, Color.Brown, Color.DarkBrown, Color.White];
    public static Color DrawingColor
    {
        get
        {
            if (colorInt >= colors.Length)
                colorInt = 0;

            return colors[colorInt];
        }

        set { }
    }

    public static int brushRadiusSelectorInt = 0;
    private static int[] radiuses = [5, 10, 15, 20, 30, 40, 50, 100];
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


    public virtual void Draw(Image canvas, Vector2 mousePos)
    {
        lastMousePos = mousePos;
    }

    public static void PreStrokeSaveCanvas(Image canvas)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            strokes.Push(Raylib.ImageCopy(canvas));
    }

    public static Image UndoStroke(Image canvas)
    {
        try
        {
            return (Raylib.IsKeyPressed(KeyboardKey.Z) && ProgramManager.popupWindow == null) ? strokes.Pop() : canvas;
        }
        catch (InvalidOperationException)
        {
            return canvas;
        }
    }

    // vvvv Tack chatgpt, youtube, stackoverflow och gud för denna algoritm nedan vvvv
    protected void DrawThickLine(Image canvas, Vector2 startPos, Vector2 endPos, Color color)
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
            // Rita en cirkel med angiven radie på den aktuella punkten
            Raylib.ImageDrawCircleV(ref canvas, new Vector2(x, y), brushRadius, color);

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
    public override void Draw(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            Raylib.ImageDrawLine(ref canvas,
                (int)lastMousePos.X, (int)lastMousePos.Y,
                (int)mousePos.X, (int)mousePos.Y,
                DrawingColor);
        }

        base.Draw(canvas, mousePos);
    }
}

public class PaintBrush : DrawTool
{
    public override void Draw(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            DrawThickLine(canvas, lastMousePos, mousePos, DrawingColor);
        }

        base.Draw(canvas, mousePos);
    }
}

public class Eraser : DrawTool
{
    public override void Draw(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            DrawThickLine(canvas, lastMousePos, mousePos, Color.White);
        }

        base.Draw(canvas, mousePos);
    }
}

public class Checker : DrawTool
{
    private const int pixelSize = 5;

    public override void Draw(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            SetCheckers(canvas, mousePos);
    }

    private void SetCheckers(Image canvas, Vector2 mousePos)
    {
        int rows = (int)Math.Ceiling((double)canvas.Height / pixelSize);
        int cols = (int)Math.Ceiling((double)canvas.Width / pixelSize);

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < cols; col++)
            {
                int xPos = col * pixelSize;
                int yPos = row * pixelSize;

                Vector2 squareCenter = new Vector2(xPos + pixelSize / 2, yPos + pixelSize / 2);

                float distanceToMouse = Vector2.Distance(mousePos, squareCenter);

                if (distanceToMouse <= brushRadius)
                {
                    if ((row + col) % 2 == 0)
                        Raylib.ImageDrawRectangle(ref canvas, xPos, yPos, pixelSize, pixelSize, DrawingColor);
                }
            }
        }
    }
}

public class Bucket : DrawTool
{
    public override void Draw(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
        {
            Color targetColor = Raylib.GetImageColor(canvas, (int)mousePos.X, (int)mousePos.Y);
            FloodFill(canvas, mousePos, targetColor);
        }
    }

    private void FloodFill(Image img, Vector2 pt, Color targetColor)
    {
        if (targetColor.Equals(DrawingColor))
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
                Raylib.ImageDrawPixel(ref img, (int)temp.X, y1, DrawingColor);

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