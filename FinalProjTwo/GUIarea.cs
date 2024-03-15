namespace DrawingProgram;

public class GUIarea : IDrawable
{
    public void Draw()
    {
        Raylib.DrawRectangle(Canvas.CanvasWidth, 0, 200, ProgramManager.ScreenHeight, Color.Gray);
        Raylib.DrawRectangle(0, Canvas.CanvasHeight, ProgramManager.ScreenWidth, 100, Color.Gray);
    }
}