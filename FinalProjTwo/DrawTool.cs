namespace DrawingProgram;

public abstract class DrawTool
{
    protected Vector2 mousePos;
    protected Vector2 lastMousePos;
    public static Stack<Image> strokes = new();
    //protected Button
    public virtual void Draw(Color color, Image canvas, int numberData)
    {
        mousePos = Raylib.GetMousePosition();
    }

    public static void SavePrevCanvas(Image canvas)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            strokes.Push(Raylib.ImageCopy(canvas));
    }

    public static Image UndoStroke(Image canvas)
    {
        try
        {
            return Raylib.IsKeyPressed(KeyboardKey.Z) ? strokes.Pop() : canvas;
        }
        catch (InvalidOperationException)
        {
            return canvas;
        }
    }
}

public class Pencil : DrawTool
{
    public override void Draw(Color drawingColor, Image canvas, int radius)
    {
        base.Draw(drawingColor, canvas, radius);

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            Raylib.ImageDrawLine(ref canvas,
                (int)lastMousePos.X, (int)lastMousePos.Y,
                (int)mousePos.X, (int)mousePos.Y,
                drawingColor);
        }

        lastMousePos = mousePos;
    }
}

public class Pen : DrawTool
{
    public override void Draw(Color drawingColor, Image canvas, int radius)
    {
        base.Draw(drawingColor, canvas, radius);

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            Raylib.ImageDrawCircleV(ref canvas, mousePos, radius, drawingColor);
        }
    }
}


//Erasing

public class Eraser : DrawTool
{
    public override void Draw(Color bgColor, Image canvas, int radius)
    {
        base.Draw(bgColor, canvas, radius);

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            Raylib.ImageDrawCircleV(ref canvas, mousePos, radius, bgColor);
        }
    }
}

public class Checker : DrawTool
{
    private const int pixelSize = 5;

    public override void Draw(Color color, Image canvas, int ditherRadius)
    {
        base.Draw(color, canvas, ditherRadius);

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            Checkerboard(mousePos, canvas, 20, Color.Black);
        }
    }

    private void Checkerboard(Vector2 mousePos, Image canvas, int brushRadius, Color color)
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
                    {
                        Raylib.ImageDrawRectangle(ref canvas, xPos, yPos, pixelSize, pixelSize, color);
                    }
                }
            }
        }
    }


}
