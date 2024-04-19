namespace DrawingProgram;

public class Icons : IDrawable
{
    public List<Texture2D> toolIcons = new();
    public List<Texture2D> miscIcons = new();

    public Icons()
    {
        toolIcons.Add(Raylib.LoadTexture("Textures/Icons/pencil.png"));
        toolIcons.Add(Raylib.LoadTexture("Textures/Icons/paintbrush.png"));
        toolIcons.Add(Raylib.LoadTexture("Textures/Icons/eraser.png"));
        toolIcons.Add(Raylib.LoadTexture("Textures/Icons/bucket.png"));
        toolIcons.Add(Raylib.LoadTexture("Textures/Icons/eyedropper.png"));
        toolIcons.Add(Raylib.LoadTexture("Textures/Icons/checkers.png"));
        toolIcons.Add(Raylib.LoadTexture("Textures/Icons/rectangle.png"));
        toolIcons.Add(Raylib.LoadTexture("Textures/Icons/line.png"));
        toolIcons.Add(Raylib.LoadTexture("Textures/Icons/circle.png"));

        miscIcons.Add(Raylib.LoadTexture("Textures/Icons/powericon.png"));
        miscIcons.Add(Raylib.LoadTexture("Textures/Icons/saveicon.png"));
        miscIcons.Add(Raylib.LoadTexture("Textures/Icons/foldericon.png"));
        miscIcons.Add(Raylib.LoadTexture("Textures/Icons/settings.png"));
        miscIcons.Add(Raylib.LoadTexture("Textures/Icons/layers.png"));

    }

    public void Draw()
    {
        for (int i = 0; i < toolIcons.Count; i++)
        {
            Raylib.DrawTexture(toolIcons[i], i * 90 + 10, Canvas.CanvasHeight + 10, Color.White);
        }
        for (int i = 0; i < miscIcons.Count - 1; i++)
        {
            Raylib.DrawTexture(miscIcons[i], Canvas.CanvasWidth + 60, i * 90 + 10, Color.White);
        }

        Raylib.DrawTexture(miscIcons[4], Canvas.CanvasWidth - 80, Canvas.CanvasHeight + 10, Color.White);
    }
}