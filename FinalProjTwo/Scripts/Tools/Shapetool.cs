namespace DrawingProgram;

public abstract class ShapeTool : DrawTool
{
    protected Vector2 startPos;
    public static bool drawFilled = true;

    public override void Update(Image canvas, Vector2 mousePos)
    {
        lock (lockObj)
        {
            base.Update(canvas, mousePos);
            DrawShape(canvas, mousePos, lastMousePos);
        }
    }

    protected virtual void DrawShape(Image canvas, Vector2 mousePos, Vector2 lastMousePos)
    {
        if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            startPos = mousePos;
    }
}