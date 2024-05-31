namespace DrawingProgram;

public sealed class LineTool : ShapeTool
{
    public static Line lineToDraw = new(new Vector2(-10000, -10000), new Vector2(-10000, -10000));

    protected override void DrawShape(Image canvas, Vector2 mousePos, Vector2 lastMousePos)
    {
        base.DrawShape(canvas, mousePos, lastMousePos);
        if (Raylib.IsMouseButtonDown(MouseButton.Left))
            lineToDraw = new(startPos, mousePos);

        if (Raylib.IsMouseButtonReleased(MouseButton.Left))
        {
            DrawThickLine(canvas, lineToDraw.StartPos, lineToDraw.EndPos, drawingColor, true);
            lineToDraw = new(new Vector2(-10000, -10000), new Vector2(-10000, -10000));
        }
    }
}