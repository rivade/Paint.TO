namespace DrawingProgram;

public class Icons : IDrawable
{
    public List<Texture2D> toolIcons = new();
    public List<Texture2D> miscIcons = new();

    public Icons()
    {
        toolIcons.Add(Raylib.LoadTexture("Icons/pencil.png"));
        toolIcons.Add(Raylib.LoadTexture("Icons/paintbrush.png"));
        toolIcons.Add(Raylib.LoadTexture("Icons/eraser.png"));
        toolIcons.Add(Raylib.LoadTexture("Icons/bucket.png"));
        toolIcons.Add(Raylib.LoadTexture("Icons/eyedropper.png"));
        toolIcons.Add(Raylib.LoadTexture("Icons/checkers.png"));
        toolIcons.Add(Raylib.LoadTexture("Icons/rectangle.png"));
        toolIcons.Add(Raylib.LoadTexture("Icons/line.png"));
        toolIcons.Add(Raylib.LoadTexture("Icons/circle.png"));

        miscIcons.Add(Raylib.LoadTexture("Icons/powericon.png"));
        miscIcons.Add(Raylib.LoadTexture("Icons/saveicon.png"));
        miscIcons.Add(Raylib.LoadTexture("Icons/foldericon.png"));

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