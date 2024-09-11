namespace DrawingProgram;

public class PerspectiveCamera
{
    public Camera2D c;
    public float relativeCanvasWidth;
    public float relativeCanvasHeight;

    public PerspectiveCamera()
    {
        c = new();
        c.Zoom = 1;
        relativeCanvasWidth = Canvas.CanvasWidth;
        relativeCanvasHeight = Canvas.CanvasHeight;
    }

    public void Logic(Vector2 mousePos)
    {
        UpdateZoom(mousePos);
        MoveCamera();
        KeepCameraOnCanvas();
    }

    private void UpdateZoom(Vector2 mousePos)
    {
        Vector2 worldPosBeforeZoom = Raylib.GetScreenToWorld2D(mousePos, c);

        if (Raylib.IsKeyPressed(KeyboardKey.Up) && c.Zoom < 5)
        {
            c.Zoom *= 1.1f;
        }
        if (Raylib.IsKeyPressed(KeyboardKey.Down) && c.Zoom > 1)
        {
            c.Zoom /= 1.1f;
        }

        Vector2 worldPosAfterZoom = Raylib.GetScreenToWorld2D(mousePos, c);
        Vector2 zoomDifference = worldPosBeforeZoom - worldPosAfterZoom;
        c.Target += zoomDifference;
    }

    private void KeepCameraOnCanvas()
    {
        if (c.Target.X < 0) c.Target.X = 0;
        if (c.Target.Y < 0) c.Target.Y = 0;

        relativeCanvasWidth = Canvas.CanvasWidth/c.Zoom;
        relativeCanvasHeight = Canvas.CanvasHeight/c.Zoom;

        if (c.Target.X + relativeCanvasWidth > Canvas.CanvasWidth) c.Target.X = Canvas.CanvasWidth - relativeCanvasWidth;
        if (c.Target.Y + relativeCanvasHeight > Canvas.CanvasHeight) c.Target.Y = Canvas.CanvasHeight - relativeCanvasHeight;
    }

    private void MoveCamera()
    {
        if (Raylib.IsKeyDown(KeyboardKey.W)) c.Target.Y -= 3;
        if (Raylib.IsKeyDown(KeyboardKey.A)) c.Target.X -= 3;
        if (Raylib.IsKeyDown(KeyboardKey.S)) c.Target.Y += 3;
        if (Raylib.IsKeyDown(KeyboardKey.D)) c.Target.X +=3;
    }

    public Vector2 projectCameraPointToCanvas(Vector2 mousePos)
    {
        return Raylib.GetScreenToWorld2D(mousePos, c);
    }
}