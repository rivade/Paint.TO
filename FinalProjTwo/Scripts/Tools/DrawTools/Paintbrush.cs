namespace DrawingProgram;

public sealed class PaintBrush : DrawTool
{
    public override void Update(Image canvas, Vector2 mousePos)
    {
        base.Update(canvas, mousePos);

        lock (lockObj)
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
                DrawThickLine(canvas, lastMousePos, mousePos, drawingColor, true);
        }
    }
}