namespace DrawingProgram;

public class Icons : IDrawable
{
    public List<Texture2D> toolIcons = new();
    public List<Texture2D> fileIcons = new();

    public Icons()
    {
        toolIcons.Add(Raylib.LoadTexture("pencil.png"));
        toolIcons.Add(Raylib.LoadTexture("paintbrush.png"));
        toolIcons.Add(Raylib.LoadTexture("checkers.png"));
        toolIcons.Add(Raylib.LoadTexture("eraser.png"));

    }

    public void Draw()
    {
        for (int i = 0; i < toolIcons.Count; i++)
        {
            Raylib.DrawTexture(toolIcons[i], i * 90 + 10, ProgramManager.CanvasHeight + 10, Color.White);
        }
        for (int i = 0; i < fileIcons.Count; i++)
        {
            Raylib.DrawTexture(fileIcons[i], ProgramManager.CanvasWidth + 10, i * 90 + 10, Color.White);
        }
    }
}