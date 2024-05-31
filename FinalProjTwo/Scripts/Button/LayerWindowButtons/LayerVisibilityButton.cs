namespace DrawingProgram;

public sealed class LayerVisibilityButton : LayerWindowButton
{
    private List<Texture2D> icons = new();
    public int currentIcon = 0;

    public LayerVisibilityButton(ProgramManager programInstance, Rectangle buttonRect, Canvas canvasInstance) : base(programInstance, buttonRect, canvasInstance)
    {
        icons.Add(Raylib.LoadTexture("Textures/Icons/visible.png"));
        icons.Add(Raylib.LoadTexture("Textures/Icons/invisible.png"));
        infoWindow = new("Toggle layer visibility", (int)buttonRect.X, (int)buttonRect.Y + ButtonSize + 5);
    }

    public override void Draw()
    {
        Raylib.DrawRectangleRec(buttonRect, GetButtonColor(Color.LightGray, Color.White, Color.White, false));
        Raylib.DrawTexture(icons[currentIcon], (int)buttonRect.X, (int)buttonRect.Y, Color.White);
        base.Draw();
    }

    public override void OnHover(Vector2 mousePos)
    {
        currentIcon = canvas.layers[canvas.currentLayer].isVisible ? 0 : 1;
        base.OnHover(mousePos);
    }

    public override void OnClick()
    {
        canvas.layers[canvas.currentLayer].isVisible = !canvas.layers[canvas.currentLayer].isVisible;
    }
}