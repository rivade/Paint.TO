namespace DrawingProgram;

public abstract class DrawTool
{
    protected Vector2 mousePos;
    protected Vector2 lastMousePos;
    protected Image canvasMemory;
    //protected Button
    public virtual void Draw(Color drawingColor, Image canvas)
    {
        mousePos = Raylib.GetMousePosition();
    }

    protected void SavePrevCanvas(Image canvas)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            canvasMemory = canvas;
    }

    public Image UndoStroke(Image canvas)
    {
        return Raylib.IsKeyPressed(KeyboardKey.Z) ? canvasMemory : canvas;
    }
}

public class Pencil : DrawTool
{
    public override void Draw(Color drawingColor, Image canvas)
    {
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            base.Draw(drawingColor, canvas);
            Raylib.ImageDrawLine(ref canvas,
            (int)lastMousePos.X, (int)lastMousePos.Y,
            (int)mousePos.X, (int)mousePos.Y,
            drawingColor);

            lastMousePos = mousePos;
        }
        else
        {
            lastMousePos = Raylib.GetMousePosition();
            if (Raylib.IsMouseButtonReleased(MouseButton.Left))
                SavePrevCanvas(canvas);
        }
    }
}

public class Eraser : DrawTool
{
    public override void Draw(Color drawingColor, Image canvas)
    {

    }
}