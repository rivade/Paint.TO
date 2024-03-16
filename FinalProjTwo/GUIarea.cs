namespace DrawingProgram;

public class GUIarea : IDrawable
{
    public static Color guiColor = Color.SkyBlue;
    public void Draw()
    {
        Raylib.DrawRectangle(Canvas.CanvasWidth, 0, 200, ProgramManager.ScreenHeight, guiColor);
        Raylib.DrawRectangle(0, Canvas.CanvasHeight, ProgramManager.ScreenWidth, 100, guiColor);
    }
}