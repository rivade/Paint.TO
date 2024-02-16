namespace DrawingProgram;

public abstract class DrawTool
{
    protected Vector2 mousePos;
    protected Vector2 lastMousePos;
    protected Image canvasMemory;
    //protected Button
    public virtual void Draw(Color drawingColor)
    {

    }

    protected void SavePrevCanvas()
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            canvasMemory = ProgramManager.canvas;
    }

    public void UndoStroke()
    {
        if (Raylib.IsKeyPressed(KeyboardKey.Z))
            ProgramManager.canvas = canvasMemory;
    }
}

public class Pencil : DrawTool
{
    public override void Draw(Color drawingColor)
    {
        base.Draw(drawingColor);

        if (Raylib.IsMouseButtonDown(MouseButton.Left))
        {
            mousePos = Raylib.GetMousePosition();
            Raylib.ImageDrawLine(ref ProgramManager.canvas,
            (int)lastMousePos.X, (int)lastMousePos.Y,
            (int)mousePos.X, (int)mousePos.Y,
            drawingColor);

            lastMousePos = mousePos;
        }
        else 
        {
            SavePrevCanvas();
            lastMousePos = Raylib.GetMousePosition();
        }
    }
}

public class Eraser : DrawTool
{
    public override void Draw(Color drawingColor)
    {

    }
}