namespace DrawingProgram;

public abstract class DrawTool
{
    protected Vector2 mousePos;
    protected Vector2 lastMousePos;
    public static Stack<Image> strokes = new();
    //protected Button
    public virtual void Draw(Color drawingColor, Image canvas)
    {
        mousePos = Raylib.GetMousePosition();
    }

    public static void SavePrevCanvas(Image canvas)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            strokes.Push(canvas);
    }

    public Image UndoStroke(Image canvas)
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
    public override void Draw(Color drawingColor, Image canvas)
    {
        base.Draw(drawingColor, canvas);

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

public class Eraser : DrawTool
{
    public override void Draw(Color drawingColor, Image canvas)
    {

    }
}