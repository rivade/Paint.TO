namespace DrawingProgram;

public sealed class Pencil : DrawTool
{
    public override void Update(Image canvas, Vector2 mousePos)
    {
        base.Update(canvas, mousePos);

        lock (lockObj)
        {
            if (Raylib.IsMouseButtonDown(MouseButton.Left))
                Raylib.ImageDrawLine(ref canvas, (int)lastMousePos.X, (int)lastMousePos.Y, (int)mousePos.X, (int)mousePos.Y, drawingColor);
        }
    }
}