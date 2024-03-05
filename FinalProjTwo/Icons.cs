namespace DrawingProgram;

public class Icons : IDrawable
{
    public List<Texture2D> toolIcons = new();
    public List<Texture2D> miscIcons = new();

    public Icons()
    {
        toolIcons.Add(Raylib.LoadTexture("pencil.png"));
        toolIcons.Add(Raylib.LoadTexture("paintbrush.png"));
        toolIcons.Add(Raylib.LoadTexture("checkers.png"));
        toolIcons.Add(Raylib.LoadTexture("eraser.png"));

        miscIcons.Add(Raylib.LoadTexture("saveicon.png"));

    }

    public void Draw()
    {
        for (int i = 0; i < toolIcons.Count; i++)
        {
            Raylib.DrawTexture(toolIcons[i], i * 90 + 10, Canvas.CanvasHeight + 10, Color.White);
        }
        for (int i = 0; i < miscIcons.Count; i++)
        {
            Raylib.DrawTexture(miscIcons[i], Canvas.CanvasWidth + 60, i * 90 + 10, Color.White);
        }
    }
}