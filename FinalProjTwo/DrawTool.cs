using System.Runtime.InteropServices;

namespace DrawingProgram;

public abstract class DrawTool
{
    protected Vector2 lastMousePos;
    public static Stack<Image> strokes = new();


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


    public virtual void Draw(Image canvas, Vector2 mousePos)
    {
        lastMousePos = mousePos;
    }

    private static Stack<Image> CleanupStrokeStack(Stack<Image> strokes)
    {
        Stack<Image> tempReverse = new();

        while (strokes.Count > 0) tempReverse.Push(strokes.Pop());

        tempReverse.Pop();

        while (tempReverse.Count > 0) strokes.Push(tempReverse.Pop());

        return strokes;
    }

    public static void PreStrokeSaveCanvas(Image canvas)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            strokes.Push(Raylib.ImageCopy(canvas));

        if (strokes.Count > 20)
            strokes = CleanupStrokeStack(strokes);
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
    public override void Draw(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            Raylib.ImageDrawLine(ref canvas,
                (int)lastMousePos.X, (int)lastMousePos.Y,
                (int)mousePos.X, (int)mousePos.Y,
                drawingColor);
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
            DrawThickLine(canvas, lastMousePos, mousePos, drawingColor, true);
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
            DrawThickLine(canvas, lastMousePos, mousePos, Color.White, true);
        }

        base.Draw(canvas, mousePos);
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


    public override void Draw(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            SetCheckers(canvas, mousePos);
    }

    private void SetCheckers(Image canvas, Vector2 mousePos)
    {
        int rows = (int)Math.Ceiling((double)canvas.Height / checkerSize);
        int cols = (int)Math.Ceiling((double)canvas.Width / checkerSize);

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

public abstract class ShapeTool : DrawTool
{
    protected enum Shapes
    {
        Rectangle,
        Line,
        Circle
    }
    protected Shapes shape;

    public static Vector2 startPos;
    public static Rectangle tempRectangle;
    public static Line tempLine = new(new Vector2(-10000, -10000), new Vector2(-10000, -10000));
    public static Circle tempCircle;

    public override void Draw(Image canvas, Vector2 mousePos)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            startPos = mousePos;

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            switch (shape)
            {
                case Shapes.Rectangle:
                    int x = Math.Min((int)startPos.X, (int)mousePos.X);
                    int y = Math.Min((int)startPos.Y, (int)mousePos.Y);
                    int width = Math.Abs((int)mousePos.X - (int)startPos.X);
                    int height = Math.Abs((int)mousePos.Y - (int)startPos.Y);
                    tempRectangle = new Rectangle(x, y, width, height);

                    break;

                case Shapes.Line:
                    tempLine = new(startPos, mousePos);
                    break;

                case Shapes.Circle:
                    tempCircle = Circle.CreateCircle(startPos, mousePos);
                    break;

            }
        }

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            switch (shape)
            {
                case Shapes.Rectangle:
                    Raylib.ImageDrawRectangle(ref canvas, (int)tempRectangle.X, (int)tempRectangle.Y,
                    (int)tempRectangle.Width, (int)tempRectangle.Height, drawingColor);

                    tempRectangle = new(Vector2.Zero, Vector2.Zero);
                    break;

                case Shapes.Line:
                    DrawThickLine(canvas, tempLine.startPos, tempLine.endPos, drawingColor, true);

                    tempLine = new(new Vector2(-10000, -10000), new Vector2(-10000, -10000));
                    break;

                case Shapes.Circle:
                    Raylib.ImageDrawCircle(ref canvas, (int)tempCircle.Middle.X, (int)tempCircle.Middle.Y, tempCircle.Radius, drawingColor);

                    tempCircle = new();
                    break;
            }
        }
    }
}

public class RectangleTool : ShapeTool
{
    public RectangleTool()
    {
        shape = Shapes.Rectangle;
    }
}

public class LineTool : ShapeTool
{
    public LineTool()
    {
        shape = Shapes.Line;
    }
}

public class CircleTool : ShapeTool
{
    public CircleTool()
    {
        shape = Shapes.Circle;
    }
}

public class EyeDropper : DrawTool
{
    public override void Draw(Image canvas, Vector2 mousePos)
    {
        Rectangle canvasRect = new(0, 0, new Vector2(canvas.Width, canvas.Height));
        if (Raylib.IsMouseButtonPressed(MouseButton.Left) && Raylib.CheckCollisionPointRec(mousePos, canvasRect))
        {
            drawingColor = Raylib.GetImageColor(canvas, (int)mousePos.X, (int)mousePos.Y);
        }
    }
}