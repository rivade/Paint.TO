namespace DrawingProgram;

public class GUIarea : IDrawable
{
    public static Color GUIColor = Color.SkyBlue;

    public void Draw()
    {
        Raylib.DrawRectangle(Canvas.CanvasWidth, 0, 200, ProgramManager.ScreenHeight, GUIColor);
        Raylib.DrawRectangle(0, Canvas.CanvasHeight, ProgramManager.ScreenWidth, 100, GUIColor);
    }
}