namespace DrawingProgram;

public class Icons : IDrawable
{
    public List<Texture2D> icons = new();

    public Icons()
    {
        icons.Add(Raylib.LoadTexture("pencil.png"));
        icons.Add(Raylib.LoadTexture("paintbrush.png"));
        icons.Add(Raylib.LoadTexture("checkers.png"));
    }

    public void DrawIcons(bool drawHorizontal)
    {
        if (drawHorizontal)
        {
            for (int i = 0; i < icons.Count; i++)
            {
                Raylib.DrawTexture(icons[i], i*90 + 10, ProgramManager.CanvasHeight + 10, Color.White);
            }
        }
    }

    public void Draw()
    {

    }
}