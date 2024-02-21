namespace DrawingProgram;

public class Canvas
{
    public static bool IsCursorOnCanvas(Vector2 mousePos)
    {
        return mousePos.X <= ProgramManager.CanvasWidth && mousePos.Y <= ProgramManager.CanvasHeight;
    }
}