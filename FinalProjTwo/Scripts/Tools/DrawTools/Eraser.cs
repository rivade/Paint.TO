namespace DrawingProgram;

public sealed class Eraser : DrawTool
{
    public override void Update(Image canvas, Vector2 mousePos)
    {
        lock (lockObj)
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
                DrawThickLine(canvas, lastMousePos, mousePos, new Color(0, 0, 0, 0), true);
        }
    }
}