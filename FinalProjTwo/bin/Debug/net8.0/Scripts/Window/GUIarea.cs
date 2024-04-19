namespace DrawingProgram;

public class GUIarea : IDrawable
{
    private static Color[] colors = [ Color.SkyBlue, Color.LightGray, Color.Gray, Color.Gold, Color.Red, Color.Pink, Color.Green ];
    public static int colorInt = 0;
    public static Color guiColor
    {
        get 
        {
            if (colorInt >= colors.Length)
                colorInt = 0;
            return colors[colorInt];
        }
    }

    public void Draw()
    {
        Raylib.DrawRectangle(Canvas.CanvasWidth, 0, 200, ProgramManager.ScreenHeight, guiColor);
        Raylib.DrawRectangle(0, Canvas.CanvasHeight, ProgramManager.ScreenWidth, 100, guiColor);
    }
}